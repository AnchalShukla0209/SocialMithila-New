using SocialMithila.Business.Interface;
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

    }
}