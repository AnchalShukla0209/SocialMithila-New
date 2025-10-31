using Antlr.Runtime.Misc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SocialMithila.Business.Business;
using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SocialMithila.Controllers
{
    public class UserController : Controller
    {
        private readonly IBllUserProfile _bll;

        public UserController(IBllUserProfile bll)
        {
            _bll = bll;
        }

        public ActionResult UserProfile(int? Id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.UserId = Id;
            using (var db = new AppDbContext())
            {
                var photos = (from p in db.Posts
                              join pm in db.PostMedia on p.PostId equals pm.PostId
                              where p.UserId == Id && pm.MediaType == "Image"
                              select pm.MediaUrl).ToList();

                ViewBag.UserPhotos = photos;
            }
            var userdata = _bll.GetUser((int)Id);
            ViewBag.About = userdata.Description ?? "";
            // 🌍 Location Data (Safe Null Handling)
            ViewBag.Address = userdata?.Address ?? "";
            ViewBag.Country = userdata?.Country ?? "";
            ViewBag.City = userdata?.TownCity ?? "";


            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var result = _bll.GetUser(userId);
            return View(result);
        }


        [HttpGet]
        public ActionResult LoadFeeds(int skip = 0, int take = 5, int id=0)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            var posts = _bll.GetFeed(userId, skip, take, id);

            return Json(posts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RenderPartial(PostFeedDTOs model)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return PartialView("~/Views/Shared/_FeedPost.cshtml", model);
        }

        [HttpPost]
        public ActionResult RenderPartial2(PostFeedDTOs model)
        {
            if (model == null || model.PostId == 0)
                return new HttpStatusCodeResult(400, "Invalid post data");
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            var feed = _bll.GetFeedById((int)model.PostId, userId);
            if (feed == null)
                return HttpNotFound();

            return PartialView("~/Views/Shared/_FeedPost.cshtml", feed);
        }




        [HttpPost]
        public void NotifyNewPost(PostFeedDTOs newPost)
        {
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<FeedHub>();
            context.Clients.All.receiveNewPost(newPost);
        }

        [HttpGet]
        public JsonResult SearchUsers(string q)
        {
            var res = _bll.SearchUsers(q);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadPost()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            string contentText = Request.Form["contentText"];

            var mentionsJson = Request.Form["mentions"];
            var mentionedIds = !string.IsNullOrEmpty(mentionsJson)
                ? JsonConvert.DeserializeObject<List<int>>(mentionsJson)
                : new List<int>();

            var files = Request.Files;
            var result = _bll.CreatePost(userId, contentText, files, mentionedIds);
            int postId = result.PostId;

            if (result != null)
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<PostHub>();
                hubContext.Clients.All.receivePost(result);

                var user = _bll.GetUser(userId); // should return { UserId, UserName, ProfilePhoto }
                string fullName = user.FirstName + (string.IsNullOrWhiteSpace(user.LastName) ? "" : " " + user.LastName);
                string message = $"{fullName} added a new post.";
                string profilePhoto = string.IsNullOrEmpty(user.ProfilePhoto)
                    ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                    : user.ProfilePhoto;

                var notifObj = new
                {
                    FromUserId = userId,
                    FromUserName = fullName,
                    ProfilePhoto = profilePhoto,
                    Message = message,
                    CreatedOn = DateTime.Now.ToString("g"),
                    Type = "NewPost"
                };


                var notif = new Notifications
                {
                    UserId = userId,
                    ActorId = userId,
                    PostId = postId,
                    Message = message,
                    NotificationType = "Newpost",
                    EntityType = "post",
                    EntityId = postId,
                    IsRead = false,
                    CreatedOn = DateTime.Now
                };


                using (var db = new AppDbContext())
                {
                    db.Notifications.Add(notif);
                    db.SaveChanges();
                }


                var notifHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notifHub.Clients.AllExcept(userId.ToString()).receiveNotification(notifObj);


                return Json(new { success = true, post = result });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult GetNotifications()
        {
            if (Session["UserId"] == null)
                return Json(new { }, JsonRequestBehavior.AllowGet);

            int userId = Convert.ToInt32(Session["UserId"].ToString());
            using (var db = new AppDbContext())
            {
                var notifications = (from n in db.Notifications
                                     join u in db.TblUser on n.ActorId equals u.Id
                                     where n.UserId == userId
                                     orderby n.NotificationId descending
                                     select new
                                     {
                                         n.PostId,
                                         n.Message,
                                         n.IsRead,
                                         CreatedOn = n.CreatedOn.HasValue ? n.CreatedOn.Value.ToString("g") : "",
                                         FromUserName = (u.FirstName + " " + (u.LastName ?? "")).Trim(),
                                         ProfilePhoto = string.IsNullOrEmpty(u.ProfilePhoto)
                                             ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                                             : u.ProfilePhoto,
                                             n.NotificationType,
                                             n.UserId,
                                             n.ActorId,
                                             n.EntityId,
                                             n.NotificationId,
                                             n.RequestStatus
                                     })
                                     .Take(10)
                                     .ToList();

                // ✅ Mark unread notifications as read
                var unread = db.Notifications.Where(n => n.UserId == userId && n.IsRead == false);
                foreach (var u in unread)
                {
                    u.IsRead = true;
                }
                db.SaveChanges();

                return Json(notifications, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UserProfilePartial(int userId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int currentuserId = Convert.ToInt32(Session["UserId"].ToString());
            var model = _bll.GetUserProfileById(userId, currentuserId);
            return PartialView("_UserProfilePartial", model);
        }

        [HttpGet]
        public ActionResult SearchUsersDetails(string searchTerm)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            using (var db = new AppDbContext())
            {
                if (string.IsNullOrEmpty(searchTerm))
                    return Json(new { users = new List<object>() }, JsonRequestBehavior.AllowGet);

                var users = db.TblUser
                    .Where(u => u.IsActive == true && u.IsDeleted == false &&
                                ((u.FirstName ?? "").Contains(searchTerm) || (u.LastName ?? "").Contains(searchTerm)))
                    .Select(u => new
                    {
                        u.Id,
                        FullName = (u.FirstName ?? "") + " " + (u.LastName ?? ""),
                        ProfilePhoto = string.IsNullOrEmpty(u.ProfilePhoto)
                            ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                            : u.ProfilePhoto
                    })
                    .Take(10)
                    .ToList();

                return Json(new { users }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadComments(long postId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            var comments = _bll.GetCommentsByPostId(postId, userId);
            ViewBag.PostId = postId;
            var userdata = _bll.GetUser(userId);
            ViewBag.CurrentUserPhoto= string.IsNullOrEmpty(userdata.ProfilePhoto)
                    ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                    : userdata.ProfilePhoto;
            return PartialView("_CommentSection", comments);
        }


        [HttpPost]
        public ActionResult SendFriendRequest(int addresseeId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int requesterId = Convert.ToInt32(Session["UserId"].ToString());
            if (addresseeId != requesterId)
            {
                
                using (var _db = new AppDbContext())
                {

                    var friendship = new Friendships
                    {
                        RequesterId = requesterId,
                        AddresseeId = addresseeId,
                        Status = "P",
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };

                    _db.Friendships.Add(friendship);
                    _db.SaveChanges();


                    var notification = new Notifications
                    {
                        UserId = addresseeId,
                        ActorId = requesterId,
                        PostId = 0,
                        Message = $"{GetUserFullName(requesterId)} sent you a friend request.",
                        NotificationType = "FriendRequest",
                        EntityType = "Friendship",
                        EntityId = friendship.FriendshipId,
                        IsRead = false,
                        CreatedOn = DateTime.Now,
                        RequestStatus="P"
                    };
                    _db.Notifications.Add(notification);
                    _db.SaveChanges();


                    var frndreqHub = GlobalHost.ConnectionManager.GetHubContext<FriendRequestHub>();
                    frndreqHub.Clients.Group(addresseeId.ToString())
                        .friendRequestUpdated(requesterId, "FriendRequest Sent");


                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    var actor = _db.TblUser.Find(requesterId);
                    hubContext.Clients.Group(addresseeId.ToString()).receiveNotification(new
                    {
                        FromUserName = GetUserFullName(requesterId),
                        Message = $"{GetUserFullName(requesterId)} sent you a friend request.",
                        ProfilePhoto = actor.ProfilePhoto,
                        CreatedOn = DateTime.Now.ToString("g")
                    });
                    return Json(new { success = true, friendshipId = friendship.FriendshipId, status = friendship.Status });
                }

                
            }
            return Json(new { success = false, friendshipId = 0, status = "Can not Sent Friend Request" });
        }

        [HttpPost]
        public JsonResult AcceptFriendRequest(int friendshipId, int addresseeId, int requesterId = 0)
        {
            using (var _db = new AppDbContext())
            {
                int currentUserId = Convert.ToInt32(Session["UserId"].ToString());
                var f = _db.Friendships.FirstOrDefault(x => x.FriendshipId == friendshipId);

                if (f == null)
                {
                    return Json(new { success = false, message = "Friend request record not present." });
                }


                if (f.Status != "P")
                {
                    return Json(new { success = false, message = "Only pending friend requests can be accepted." });
                }


                f.Status = "A";
                f.UpdatedOn = DateTime.Now;
                _db.SaveChanges();

                var notificationData = _db.Notifications.Where(id => id.EntityId == friendshipId && id.RequestStatus == "P").FirstOrDefault();
                if(notificationData!=null)
                {
                    notificationData.RequestStatus = "A";
                    _db.Notifications.Update(notificationData);
                    _db.SaveChanges();

                }

                var notification = new Notifications
                {
                    PostId = 0,
                    Message = $"{GetUserFullName(currentUserId)} accepted your friend request.",
                    UserId = f.RequesterId,
                    ActorId = currentUserId,
                    NotificationType = "FriendAccept",
                    EntityType = "Friendship",
                    EntityId = f.FriendshipId,
                    IsRead = false,
                    CreatedOn = DateTime.Now,
                    RequestStatus="A"
                };
                _db.Notifications.Add(notification);
                _db.SaveChanges();

                if (requesterId == 0)
                {
                    var frndreqHub = GlobalHost.ConnectionManager.GetHubContext<FriendRequestHub>();
                    frndreqHub.Clients.Group(addresseeId.ToString())
                        .friendRequestUpdated(currentUserId, "FriendRequest Accepted");
                }
                else
                {
                    var frndreqHub = GlobalHost.ConnectionManager.GetHubContext<FriendRequestHub>();
                    frndreqHub.Clients.Group(addresseeId.ToString())
                        .friendRequestUpdated(requesterId, "FriendRequest Accepted");
                }

                var actor = _db.TblUser.Find(currentUserId);

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                hubContext.Clients.Group(f.RequesterId.ToString()).receiveNotification(new
                {
                    FromUserName = GetUserFullName(currentUserId),
                    Message = $"{GetUserFullName(currentUserId)} accepted your friend request.",
                    ProfilePhoto = actor.ProfilePhoto,
                    CreatedOn = DateTime.Now.ToString("g")
                });


                return Json(new { success = true, status = "A" });
            }
        }

        [HttpPost]
        public ActionResult CancelFriendRequest(long friendshipId, int addresseeId, int requesterId = 0)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"].ToString());

                using (var db = new AppDbContext())
                {
                    var request = db.Friendships.FirstOrDefault(f => f.FriendshipId == friendshipId);


                    if (request == null)
                        return Json(new { success = false, message = "Friend request record not present." });

                    if (request.RequesterId != userId && request.AddresseeId != userId)
                        return Json(new { success = false, message = "Unauthorized operation." });

                    if (request.Status != "P")
                        return Json(new { success = false, message = "Only pending friend requests can be deleted." });


                    db.Friendships.Remove(request);
                    db.SaveChanges();

                    if (requesterId == 0)
                    {
                        var frndreqHub = GlobalHost.ConnectionManager.GetHubContext<FriendRequestHub>();
                        frndreqHub.Clients.Group(addresseeId.ToString())
                            .friendRequestUpdated(userId, "FriendRequest Deleted");
                    }
                    else
                    {
                        var frndreqHub = GlobalHost.ConnectionManager.GetHubContext<FriendRequestHub>();
                        frndreqHub.Clients.Group(addresseeId.ToString())
                            .friendRequestUpdated(requesterId, "FriendRequest Deleted");
                    }

                    var notificationData = db.Notifications.Where(id => id.EntityId == friendshipId && id.RequestStatus == "P").FirstOrDefault();
                    if (notificationData != null)
                    {
                        notificationData.RequestStatus = "R";
                        db.Notifications.Update(notificationData);
                        db.SaveChanges();

                    }

                    ////var notification = new Notifications
                    ////{
                    ////    UserId = addresseeId,
                    ////    ActorId = userId,
                    ////    PostId = 0,
                    ////    Message = $"{GetUserFullName(userId)} deleted friend request.",
                    ////    NotificationType = "FriendRequest",
                    ////    EntityType = "Friendship",
                    ////    EntityId = friendshipId,
                    ////    IsRead = false,
                    ////    CreatedOn = DateTime.Now,
                    ////    RequestStatus = "R"
                    ////};
                    ////db.Notifications.Add(notification);
                    ////db.SaveChanges();

                    //var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    //var actor = db.TblUser.Find(userId);
                    //if (requesterId == 0)
                    //{
                    //    hubContext.Clients.Group(addresseeId.ToString()).receiveNotification(new
                    //    {
                    //        FromUserName = GetUserFullName(userId),
                    //        Message = $"{GetUserFullName(userId)} deleted friend request.",
                    //        ProfilePhoto = actor.ProfilePhoto,
                    //        CreatedOn = DateTime.Now.ToString("g")
                    //    });
                    //}
                    //else
                    //{
                    //    hubContext.Clients.Group(requesterId.ToString()).receiveNotification(new
                    //    {
                    //        FromUserName = GetUserFullName(userId),
                    //        Message = $"{GetUserFullName(userId)} deleted friend request.",
                    //        ProfilePhoto = actor.ProfilePhoto,
                    //        CreatedOn = DateTime.Now.ToString("g")
                    //    });
                    //}

                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Server error: " + ex.Message });
            }
        }




        private string GetUserFullName(int userId)
        {
            using (var _db = new AppDbContext())
            {
                var u = _db.TblUser.FirstOrDefault(x => x.Id == userId);
                return (u?.FirstName + " " + u?.LastName)?.Trim();
            }
        }



        [HttpPost]
        public ActionResult AddComment(CommentsRequestModel COM)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            COM.UserId = userId;
            var res = _bll.AddComment(COM);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RenderPost(int postId)
        {
            if (postId == 0)
                return new HttpStatusCodeResult(400, "Invalid post data");
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());

            var feed = _bll.GetFeedById((int)postId, userId);
            if (feed == null)
                return HttpNotFound();

            return PartialView("~/Views/Shared/_FeedPost.cshtml", feed);
        }


        [HttpPost]
        public ActionResult MarkNotificationRead(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            using (var _db = new AppDbContext())
            {
                var notification = _db.Notifications.FirstOrDefault(n => n.NotificationId == id && n.UserId == userId);
                if (notification == null)
                    return HttpNotFound("Notification not found");

                notification.IsRead = true;
                _db.SaveChanges();
            }
            return Json(new { success = true, message = "Notification marked as read" });
        }

        [HttpPost]
        public ActionResult MarkAllNotificationsRead()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            using (var _db = new AppDbContext())
            {
                int userId = Convert.ToInt32(Session["UserId"]);

                var notifications = _db.Notifications.Where(n => n.UserId == userId && n.IsRead==false).ToList();
                notifications.ForEach(n => n.IsRead = true);

                _db.SaveChanges();

                return Json(new { success = true, message = "All notifications marked as read" });
            }
        }

        [HttpGet]
        public ActionResult SocialAccount()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
    }

}
