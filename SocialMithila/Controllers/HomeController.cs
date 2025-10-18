using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialMithila.Business.Business;
using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SocialMithila.Controllers
{
    public class HomeController : Controller
    {
        public IBllHome _IBllHome { get; set; }
        public IBllUserProfile _IBllUserProfile { get; set; }
        private readonly IBllBusiness _businessBAL;
        public HomeController(IBllHome objIBllHome, IBllUserProfile objIBllUserProfile, IBllBusiness businessBAL)
        {
            _IBllHome = objIBllHome;
            _IBllUserProfile = objIBllUserProfile;
            _businessBAL = businessBAL;
        }
        public async Task<ActionResult> Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            ViewBag.UserId = userId;
            using (var db = new AppDbContext())
            {
                var result = await db.TblUser
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.ProfilePhoto
                    })
                    .ToListAsync();

                if (result == null || result.Count == 0)
                {
                    ViewBag.Message = "No users found.";
                    return View();
                }
                // ✅ Pass the full list to the View
                ViewBag.Users = result;

                // Optional: Logged-in user's info
                var currentUser = result.FirstOrDefault(u => u.Id == userId);
                if (currentUser != null)
                {
                    ViewBag.FullName = currentUser.FirstName + " " + currentUser.LastName;
                    ViewBag.ProfileImage = string.IsNullOrEmpty(currentUser.ProfilePhoto)
                        ? "default.png"
                        : currentUser.ProfilePhoto;
                }
                var results = _IBllUserProfile.GetUser(userId);
                ViewBag.Categories = await _businessBAL.GetCategoriesAsync();
                ViewBag.PaymentMethods = await _businessBAL.GetPaymentMethodsAsync();
                return View(results);
            }
        }


        public ActionResult DefaultIndex()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Usersetting()
        {
            return View();
        }

        public ActionResult Accountinformation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Accountinformation(UserDTO user, HttpPostedFileBase profilePhotoFile)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            user.Id = UserId;
            var res = _IBllHome.Accountinformation(user, profilePhotoFile);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAccountinformation()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var user = _IBllHome.GetAccountinformation(UserId);
            Session["UserName"] = user.Data.FristNAme+" "+ user.Data.LastNAme;
            Session["UserProfile"] = string.IsNullOrEmpty(user.Data.ProfilePhoto)? "https://cdn-icons-png.flaticon.com/512/149/149071.png" : user.Data.ProfilePhoto;
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFriendRequest()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var user = _IBllHome.GetFriendRequest(UserId);
            return Json(user, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddStory(decimal Latitude, decimal Longitude, HttpPostedFileBase profilePhotoFile)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var res = _IBllHome.AddStory(Latitude, Longitude, profilePhotoFile, UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllUserStories()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }
                int loggedInUserId = Convert.ToInt32(Session["UserId"].ToString());
                var result = _IBllHome.GetAllUserStories(loggedInUserId);
                return Json(new { loggedInUserId, result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetStoriesByUser(int userId)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }
                int loggedInUserId = Convert.ToInt32(Session["UserId"].ToString());
                var result = _IBllHome.GetStoriesByUser(userId, loggedInUserId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetStoryById(int storyId)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }
                int loggedInUserId = Convert.ToInt32(Session["UserId"].ToString());
                var result = _IBllHome.GetStoriesBStoryId(storyId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LikePost(long postId, string reactionType)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int loggedInUserId = Convert.ToInt32(Session["UserId"].ToString());
            var post = _IBllHome.AddLike(postId, loggedInUserId, reactionType);
            if (post != null)
            {
                using (var db = new AppDbContext())
                {
                    string ReactionType = db.TblPostLike.Where(x => x.PostId == postId).OrderByDescending(x => x.Id).Select(x => x.ReactionType).FirstOrDefault();
                    var context = GlobalHost.ConnectionManager.GetHubContext<FeedHub>();
                    context.Clients.Group(post.UserId.ToString()).updateLikeCount(postId, ReactionType);

                    var postData = _IBllUserProfile.GetFeedById((int)postId, loggedInUserId);
                    var FBcontext = GlobalHost.ConnectionManager.GetHubContext<FeedHub>();
                    FBcontext.Clients.Group(post.UserId.ToString()).UpdateReaction(postId, postData.LikeCount, postData.UserReactions);
                    post.ReactionType = ReactionType;
                    post.UserReactions = postData.UserReactions;
                }
                //

                if (loggedInUserId != post.UserId)
                {
                    var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    hubContext.Clients.Group(post.UserId.ToString())
                        .receiveNotification(post.msg);
                }

            }
            return Json(new { success = true, data = post });

        }


        [HttpPost]
        public ActionResult LikeComment(long commentId)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Auth");

            int loggedInUserId = Convert.ToInt32(Session["UserId"].ToString());

            // Generic method for comment or sub-reply
            var result = _IBllHome.AddCommentLike(commentId, loggedInUserId);

            if (result != null)
            {
                using (var db = new AppDbContext())
                {
                    // Real-time SignalR update
                    var likeCount = db.TblPostLike.Count(x => x.CommentId == commentId);
                    var comment = db.Comments.FirstOrDefault(x => x.CommentId == commentId);

                    if (comment != null)
                    {
                        var feedHub = GlobalHost.ConnectionManager.GetHubContext<FeedHub>();
                        feedHub.Clients.Group(comment.UserId.ToString())
                            .updateCommentLike(commentId, likeCount);

                        if (loggedInUserId != comment.UserId)
                        {
                            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                            notificationHub.Clients.Group(comment.UserId.ToString())
                                .receiveNotification(result.msg);
                        }
                    }
                }

                return Json(new { success = true, data = result });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult StoryLike(int storyId)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Auth");

            int loggedInUserId = Convert.ToInt32(Session["UserId"].ToString());

            // Generic method for comment or sub-reply
            var result = _IBllHome.StoryLike(storyId, loggedInUserId);

            if (result != null)
            {
                using (var db = new AppDbContext())
                {
                    // Real-time SignalR update
                    var likeCount = db.TblStoryLike.Count(x => x.StoryId == storyId);
                    var stories = db.Stories.FirstOrDefault(x => x.StoryId == storyId);

                    if (stories != null)
                    {
                        var feedHub = GlobalHost.ConnectionManager.GetHubContext<FeedHub>();
                        feedHub.Clients.Group(stories.UserId.ToString())
                            .updateStoryLike(stories, likeCount);

                        if (loggedInUserId != stories.UserId)
                        {
                            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                            notificationHub.Clients.Group(stories.UserId.ToString())
                                .receiveNotification(result.msg);
                        }
                    }
                }

                return Json(new { success = true, data = result });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult MarkStoryViewed(long storyId)
        {
            if (Session["UserId"] == null)
                return Json(new { success = false });

            int userId = Convert.ToInt32(Session["UserId"].ToString());

            using (var db = new AppDbContext())
            {
                var existing = db.TblStoryView
                    .FirstOrDefault(x => x.StoryId == storyId && x.ViewedBy == userId);

                if (existing == null)
                {
                    db.TblStoryView.Add(new TblStoryView
                    {
                        StoryId = storyId,
                        ViewedBy = userId,
                        ViewedOn = DateTime.Now
                    });
                    db.SaveChanges();
                }
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public JsonResult GetStoryViewers(long storyId)
        {
            using (var db = new AppDbContext())
            {
                var viewers = (from v in db.TblStoryView
                               join u in db.TblUser on v.ViewedBy equals u.Id
                               where v.StoryId == storyId
                               orderby v.ViewedOn descending
                               select new
                               {
                                   u.Id,
                                   Name = u.FirstName ?? "" + " " + u.LastName ?? "",
                                   u.ProfilePhoto,
                                   v.ViewedOn,
                                   v.IsLiked
                               }).ToList();

                return Json(new { success = true, viewers }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetFriends(string search = "", int page = 1, int pageSize = 5)
        {
            try
            {
                if(search=="undefined")
                {
                    search = "";
                }
                if (Session["UserId"] == null)
                    return RedirectToAction("Login", "Auth");

                int currentUserId = Convert.ToInt32(Session["UserId"]);

                using (var _db = new AppDbContext())
                {
                    // 1️⃣ Friends where current user is Requester
                    var requesterFriends = _db.Friendships
                        .Where(f => f.RequesterId == currentUserId && f.Status == "A")
                        .Select(f => f.Addressee);

                    // 2️⃣ Friends where current user is Addressee
                    var addresseeFriends = _db.Friendships
                        .Where(f => f.AddresseeId == currentUserId && f.Status == "A")
                        .Select(f => f.Requester);

                    // 3️⃣ Union both
                    var friendsQuery = requesterFriends
                        .Union(addresseeFriends)
                        .AsQueryable();

                    // 4️⃣ Apply search filter
                    if (!string.IsNullOrEmpty(search))
                    {
                        friendsQuery = friendsQuery
                            .Where(u => (u.FirstName + " " + (u.LastName ?? "")).Contains(search));
                    }

                    // 5️⃣ Total count
                    var total = friendsQuery.Count();
                    var totalPages = (int)Math.Ceiling((double)total / pageSize);

                    // 6️⃣ Fetch paged users
                    var users = friendsQuery
                        .OrderBy(u => u.FirstName)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    // 7️⃣ Project to ViewModel
                    var data = users.Select(u => new FriendViewModel
                    {
                        UserId = u.Id,
                        FullName = (u.FirstName + " " + (u.LastName ?? "")).Trim(),
                        ProfilePhotoUrl = string.IsNullOrEmpty(u.ProfilePhoto)
                                          ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                                          : u.ProfilePhoto,
                        MutualFriends = (u.FriendshipsAddressee?.Count() ?? 0) + (u.FriendshipsRequester?.Count() ?? 0)
                    }).ToList();

                    return Json(new { data, total, page, totalPages }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }


    }


}
