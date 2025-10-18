using Microsoft.EntityFrameworkCore;
using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.RequestModel;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialMithila.Controllers
{
    public class BusinessController : Controller
    {
        private const string DefaultImage = "https://cdn-icons-png.flaticon.com/512/407/407861.png";
        private const string DefaultFollowerImage = "https://cdn-icons-png.flaticon.com/512/149/149071.png";

        private readonly IBllBusiness _businessBAL;

        public BusinessController(IBllBusiness businessBAL)
        {
            _businessBAL = businessBAL;
        }


        [HttpGet]
        public async Task<JsonResult> GetSubCategories(int categoryId)
        {
            var subCategories = await _businessBAL.GetSubCategoriesByCategoryAsync(categoryId);
            return Json(subCategories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddBusiness(SocialMithila.SharedDataAccess.EFCore.Business business)
        {
            var uploadedFiles = new List<string>();
            var imageUrls = new List<string>();

            try
            {
                int userId = Convert.ToInt32(Session["UserId"] ?? "0");
                business.userid = userId;
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string uploadsFolder = Path.Combine(basePath, "Content", "UploadFolder", "BusinessShops", userId.ToString());

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var files = Request.Files;

                if (files.Count == 0)
                    return Json(new { success = false, message = "Please upload at least one image (480x320)." });

                for (int i = 0; i < files.Count && i < 5; i++)
                {
                    var file = files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        using (var img = Image.FromStream(file.InputStream))
                        {
                            if (img.Width != 480 || img.Height != 320)
                                return Json(new { success = false, message = "Each image must be exactly 480x320 pixels." });
                        }

                        file.InputStream.Position = 0;
                        string extension = Path.GetExtension(file.FileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        file.SaveAs(filePath);
                        uploadedFiles.Add(filePath);
                        string mediaUrl = $"/Content/UploadFolder/BusinessShops/{userId}/{uniqueFileName}";
                        imageUrls.Add(mediaUrl);
                    }
                }

                if (!imageUrls.Any())
                    return Json(new { success = false, message = "Please upload at least one valid image (500x500)." });

                if (!string.IsNullOrWhiteSpace(Request.Form["Latitude"]))
                {
                    if (decimal.TryParse(Request.Form["Latitude"], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lat))
                        business.Latitude = lat;
                    else
                        business.Latitude = 0;
                }

                if (!string.IsNullOrWhiteSpace(Request.Form["Longitude"]))
                {
                    if (decimal.TryParse(Request.Form["Longitude"], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lng))
                        business.Longitude = lng;
                    else
                        business.Longitude = 0;
                }

                var amenities = new List<string>();

                if (Request.Form.GetValues("Aminities[]") != null)
                {
                    amenities = Request.Form.GetValues("Aminities[]")
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x.Trim()) // remove extra spaces
                        .Distinct(StringComparer.OrdinalIgnoreCase) // ignore case
                        .ToList();
                }

                
                await _businessBAL.AddBusinessAsync(business, imageUrls, amenities);

                return Json(new { success = true, message = "Business added successfully!" });
            }
            catch (Exception ex)
            {
                foreach (var path in uploadedFiles)
                {
                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                    catch { }
                }

                return Json(new { success = false, message = "Error while saving business: " + ex.Message });
            }
        }


        public ActionResult GetAllBusinesses(int page = 1, int pageSize = 5, string sortBy = "relevance", string search = "")
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var model = _businessBAL.GetAllBusinesses(userId, page, pageSize, sortBy, search);
            return PartialView("_BusinessListPartial", model);
        }

        [HttpPost]
        public ActionResult ToggleFollow(int id)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }
                int userId = Convert.ToInt32(Session["UserId"].ToString());
                bool isFollowing = _businessBAL.ToggleFollow(userId, id);

                return Json(new { success = true, isFollowing });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult BusinessDetails(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var businessdata = _businessBAL.GetBusinessById((int)id);
            return View(businessdata);
            
        }

        [HttpPost]
        public ActionResult AddReview(BusinessReview model)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                using (var db = new AppDbContext())
                {
                    model.CreatedAt = DateTime.Now;
                    model.userid = Convert.ToInt32(Session["UserId"]);
                    db.BusinessReviews.Add(model);
                    db.SaveChanges();

                    var user = db.TblUser.FirstOrDefault(u => u.Id == model.userid);

                    // ✅ Recalculate after saving
                    var averageRating = db.BusinessReviews
                        .Where(r => r.BusinessId == model.BusinessId)
                        .Average(r => (decimal?)r.Rating) ?? 0;

                    var totalRatings = db.BusinessReviews
                        .Count(r => r.BusinessId == model.BusinessId);

                    // ✅ Prepare data for return
                    var data = new
                    {
                        ProfilePhoto = user?.ProfilePhoto ?? "https://cdn-icons-png.flaticon.com/512/149/149071.png",
                        UserName = model.UserName,
                        CreatedAt = model.CreatedAt?.ToString("dd MMM yyyy hh:mm tt"),
                        Rating = model.Rating,
                        ReviewText = model.ReviewText,
                        AverageRating = Math.Round(averageRating, 2),
                        TotalRatings = totalRatings
                    };

                    return Json(new { success = true, data });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult AddContact(int BusinessId, string Name, string Email, string MobileNumber, string Message)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return Json(new { success = false, message = "Please login to contact the business." });
                }

                using (var db = new AppDbContext())
                {
                    // 1️⃣ Save contact entry
                    var contact = new Contact
                    {
                        Name = Name,
                        Email = Email,
                        MobileNumber = MobileNumber,
                        Message = Message,
                        IsDeleted = false,
                        UserId = Convert.ToInt32(Session["UserId"])
                    };
                    db.Contact.Add(contact);
                    db.SaveChanges();

                    // 2️⃣ Get business owner's email
                    var business = db.Businesses.FirstOrDefault(b => b.BusinessId == BusinessId);
                    if (business == null)
                        return Json(new { success = false, message = "Business not found." });

                    var businessOwner = db.TblUser.FirstOrDefault(u => u.Id == business.userid);
                    if (businessOwner == null)
                        return Json(new { success = false, message = "Business owner not found." });

                    // 3️⃣ Send email
                    SendContactMail(businessOwner.Email, Name, Email, MobileNumber, Message);

                    return Json(new { success = true, message = "Your message has been sent successfully!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private void SendContactMail(string toEmail, string name, string email, string mobile, string message)
        {
            string fromEmail = "socialmithila2@gmail.com";
            string fromPassword = "lzcn qrod etxh zbmm"; // ⚠️ In production, use secrets/env variables

            string subject = $"New Contact Query from {name}";

            // ✨ Beautiful HTML template
            string body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Segoe UI', sans-serif;
                    background-color: #f8f9fa;
                    padding: 20px;
                    color: #333;
                }}
                .card {{
                    background-color: #fff;
                    border-radius: 10px;
                    padding: 20px;
                    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                    max-width: 600px;
                    margin: auto;
                }}
                h2 {{
                    color: #007bff;
                    margin-bottom: 10px;
                }}
                .info {{
                    margin: 8px 0;
                    line-height: 1.6;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 12px;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='card'>
                <h2>📩 New Business Contact Inquiry</h2>
                <p class='info'><strong>Name:</strong> {name}</p>
                <p class='info'><strong>Email:</strong> {email}</p>
                <p class='info'><strong>Mobile:</strong> {mobile}</p>
                <p class='info'><strong>Message:</strong></p>
                <p class='info' style='border-left: 4px solid #007bff; padding-left: 10px;'>{message}</p>
                <div class='footer'>
                    <p>Message received via <strong>SocialMithila</strong> Contact Form.</p>
                </div>
            </div>
        </body>
        </html>";

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            using (var mail = new MailMessage(fromEmail, toEmail, subject, body))
            {
                mail.IsBodyHtml = true;
                smtp.Send(mail);
            }
        }


        
        public ActionResult BusinessList()
        {
            
           // var businessdata = _businessBAL.BusinessList();
            return View();

        }

        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            using (var _context = new AppDbContext())
            {
                var categories = await _context.Categories
                    .Select(c => new
                    {
                        c.CategoryId,
                        c.CategoryName
                    })
                    .OrderBy(c => c.CategoryName)
                    .ToListAsync();

                return Json(new { success = true, data = categories }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetSubCategory()
        {
            using (var reader = new StreamReader(Request.InputStream))
            {
                string body = await reader.ReadToEndAsync();

                // Deserialize the incoming JSON array into a list of integers
                var categoryIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(body);

                using (var _context = new AppDbContext())
                {
                    if (categoryIds == null || !categoryIds.Any())
                    {
                        return Json(new { success = false, data = new List<object>() }, JsonRequestBehavior.AllowGet);
                    }

                    var subCategories = await _context.SubCategories
                        .Where(s => categoryIds.Contains(s.CategoryId))
                        .Select(s => new
                        {
                            s.SubCategoryId,
                            s.SubCategoryName
                        })
                        .OrderBy(s => s.SubCategoryName)
                        .ToListAsync();

                    return Json(new { success = true, data = subCategories }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetShops()
        {
            using (var _context = new AppDbContext())
            {
                var shops = await _context.Businesses
                    .Where(b => b.BusinessName != null)
                    .Select(b => new
                    {
                        b.BusinessId,
                        b.BusinessName,
                        ImageUrl = b.BusinessImages
                                    .OrderBy(img => img.ImageId)
                                    .Select(img => img.ImageUrl)
                                    .FirstOrDefault() ?? b.ImageUrl ?? DefaultImage
                    })
                    .OrderBy(x => x.BusinessName)
                    .ToListAsync();

                return Json(new { success = true, data = shops }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFollowers(int? businessId = null)
        {
            using (var _context = new AppDbContext())
            {
                // Join followers with users, filter active users and optional businessId
                var query = from ubf in _context.user_business_follows
                            join u in _context.TblUser on ubf.UserId equals u.Id
                            where u.IsActive == true
                                  && (businessId == null || ubf.BusinessId == businessId.Value)
                            select new
                            {
                                u.Id,
                                FullName = (u.FirstName ?? "") + (string.IsNullOrEmpty(u.LastName) ? "" : " " + u.LastName),
                                Photo = u.ProfilePhoto
                            };

                // Distinct by user id and order by name
                var followers = await query
                    .GroupBy(x => new { x.Id, x.FullName, x.Photo })
                    .Select(g => new
                    {
                        Id = g.Key.Id,
                        FullName = g.Key.FullName,
                        Photo = g.Key.Photo
                    })
                    .OrderBy(x => x.FullName)
                    .ToListAsync();

                // Ensure fallback image
                var result = followers.Select(f => new
                {
                    Id = f.Id,
                    FullName = string.IsNullOrWhiteSpace(f.FullName) ? "Unknown" : f.FullName,
                    ImageUrl = string.IsNullOrWhiteSpace(f.Photo) ? DefaultFollowerImage : f.Photo
                }).ToList();

                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetBusinesses(BusinessFilterRequest filter)
        {
            try
            {
                using (var _db = new AppDbContext())
                {
                    const string DefaultImage = "https://cdn-icons-png.flaticon.com/512/407/407861.png";
                    int pageSize = 6;
                    int page = filter.Page <= 0 ? 1 : filter.Page;

                    var subCategoryIds = filter.SubCategoryIds ?? new List<int>();
                    var shopIds = filter.ShopIds ?? new List<int>();
                    var ratingIds = filter.Ratings ?? new List<int>();
                    var followerIds = filter.FollowerIds ?? new List<int>();

                    var query = _db.Businesses
                        .Include(b => b.BusinessImages)
                        .Include(b => b.Category)
                        .Include(b => b.SubCategory)
                        .Include(b => b.BusinessReviews)
                        .AsQueryable();

                    // If ShopIds provided, prefer them (skip category/subcategory filtering)
                    if ((shopIds == null || !shopIds.Any()) && filter.CategoryId.HasValue && filter.CategoryId.Value > 0)
                        query = query.Where(b => b.CategoryId == filter.CategoryId.Value);

                    if ((shopIds == null || !shopIds.Any()) && subCategoryIds.Any())
                        query = query.Where(b => b.SubCategoryId.HasValue && subCategoryIds.Contains(b.SubCategoryId.Value));

                    if (shopIds.Any())
                        query = query.Where(b => shopIds.Contains(b.BusinessId));

                    if (ratingIds.Any())
                    {
                        query = query.Where(b =>
                            b.BusinessReviews.Any(r =>
                                ratingIds.Any(rid => r.Rating >= rid && r.Rating < rid + 1)
                            )
                        );
                    }




                    if (followerIds.Any())
                    {
                        var followedBusinessIds = _db.user_business_follows
                            .Where(f => followerIds.Contains(f.UserId))
                            .Select(f => f.BusinessId)
                            .Distinct()
                            .ToList();

                        query = query.Where(b => followedBusinessIds.Contains(b.BusinessId));
                    }

                    int totalCount = query.Count();
                    int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                   
                    var raw = query
                        .OrderByDescending(b => b.CreatedAt)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(b => new
                        {
                            b.BusinessId,
                            b.BusinessName,
                            b.Address,
                            BusinessRating = b.Rating,
                            ReviewsAvg = b.BusinessReviews.Select(r => (double?)r.Rating).Average(),
                            ImageUrl = b.BusinessImages.Select(i => i.ImageUrl).FirstOrDefault() ?? DefaultImage
                        })
                        .ToList();
                    var data = raw.Select(x =>
                    {
                        decimal finalRating;

                        if (x.BusinessRating.HasValue && x.BusinessRating.Value > 0)
                        {
                           
                            finalRating = x.BusinessRating.Value;
                        }
                        else if (x.ReviewsAvg.HasValue)
                        {
                            finalRating = Math.Round((decimal)x.ReviewsAvg.Value, 1);
                        }
                        else
                        {
                            finalRating = 0m;
                        }

                        return new
                        {
                            x.BusinessId,
                            x.BusinessName,
                            x.Address,
                            Rating = finalRating,
                            ImageUrl = x.ImageUrl
                        };
                    }).ToList();

                    return Json(new
                    {
                        success = true,
                        data,
                        page,
                        totalPages
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}