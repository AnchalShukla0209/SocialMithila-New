using Microsoft.AspNetCore.Http;
using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.DataAccess.ResponseModel.Common;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SocialMithila.Business.Business
{
    public class BllHome : IBllHome
    {
        private readonly AppDbContext _db;
        public BllHome()
        {
            _db = new AppDbContext();
        }
        public CommonResponse Accountinformation(UserDTO user, HttpPostedFileBase profilePhotoFile)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                using (var context = new AppDbContext())
                {
                    var existingUser = context.TblUser.FirstOrDefault(x => x.Id == user.Id);

                    if (existingUser != null)
                    {
                        if (profilePhotoFile != null && profilePhotoFile.ContentLength > 0)
                        {

                            string basePath = AppDomain.CurrentDomain.BaseDirectory;


                            string uploadsFolder = Path.Combine(basePath, "Content", "UploadFolder", "ProfilePhoto", user.Id.ToString());

                            if (!Directory.Exists(uploadsFolder))
                                Directory.CreateDirectory(uploadsFolder);

                            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePhotoFile.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                            profilePhotoFile.SaveAs(filePath);


                            existingUser.ProfilePhoto = $"/Content/UploadFolder/ProfilePhoto/{user.Id}/{uniqueFileName}";

                        }


                        // ✅ Update other fields
                        existingUser.FirstName = user.FristNAme;
                        existingUser.LastName = user.LastNAme ?? "";
                        existingUser.Phone = user.Phone ?? "";
                        existingUser.Country = user.Country ?? "";
                        existingUser.Address = user.Address ?? "";
                        existingUser.TownCity = user.TownCity ?? "";
                        existingUser.PostCode = user.PostCode ?? "";
                        existingUser.Description = user.Description ?? "";

                        context.TblUser.Update(existingUser);
                        response.msg = "Profile updated successfully";
                    }

                    context.SaveChanges();
                    response.success = true;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.msg = "Error: " + ex.Message;
            }

            return response;
        }


        public GetAccountinformationDetails GetAccountinformation(int Id)
        {
            GetAccountinformationDetails response = new GetAccountinformationDetails();

            var user = _db.TblUser.FirstOrDefault(x => x.Id == Id);

            if (user == null)
            {
                response.Success = false;
                response.Msg = "User not found";
                response.Data = null;
                return response;
            }

            var userDto = new UserDTO
            {
                FristNAme = user.FirstName,
                LastNAme = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Country = user.Country,
                Address = user.Address,
                TownCity = user.TownCity,
                PostCode = user.PostCode,
                Description = user.Description,
                ProfilePhoto = user.ProfilePhoto
            };

            response.Success = true;
            response.Msg = "User fetched successfully";
            response.Data = userDto;

            return response;
        }


        public GetGetfriendRequestDetails GetFriendRequest(int userId)
        {
            var response = new GetGetfriendRequestDetails();
            
            var requests = (from fr in _db.Friendships
                            join u in _db.TblUser on fr.RequesterId equals u.Id
                            where fr.AddresseeId == userId
                                  && fr.Status == "P"
                                  && u.IsActive == true
                                  && u.IsDeleted == false
                            select new GetfriendRequest
                            {
                                AddresseeId = fr.AddresseeId,
                                RequesterId = fr.RequesterId,
                                FriendshipId = (int?)fr.FriendshipId,
                                FristNAme = u.FirstName,
                                LastNAme = u.LastName,
                                ProfilePhoto = u.ProfilePhoto
                            }).ToList();

            if (requests != null && requests.Count > 0)
            {
                response.Success = true;
                response.Msg = "Success";
                response.Data = requests;
            }
            else
            {
                response.Success = false;
                response.Msg = "No pending friend requests found.";
                response.Data = new List<GetfriendRequest>();
            }

            return response;
        }


        public CommonResponse AddStory(decimal Latitude, decimal Longitude, HttpPostedFileBase profilePhotoFile, int userId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (profilePhotoFile == null || profilePhotoFile.ContentLength <= 0)
                {
                    response.success = false;
                    response.msg = "Please upload a media file.";
                    return response;
                }

                string extension = Path.GetExtension(profilePhotoFile.FileName).ToLower();
                string mediaType = "";
                string folderType = "";

                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                {
                    mediaType = "image";
                    folderType = "Image";
                }
                else if (extension == ".mp4" || extension == ".mov" || extension == ".avi" || extension == ".mkv")
                {
                    mediaType = "video";
                    folderType = "Video";
                }
                else
                {
                    response.success = false;
                    response.msg = "Invalid file type. Only images and videos are allowed.";
                    return response;
                }

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string uploadsFolder = Path.Combine(basePath, "Content", "UploadFolder", "Stories", folderType, userId.ToString());

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                profilePhotoFile.SaveAs(filePath);

                string mediaUrl = $"/Content/UploadFolder/Stories/{folderType}/{userId}/{uniqueFileName}";

                using (var db = new AppDbContext())
                {
                    var story = new Stories
                    {
                        UserId = userId,
                        MediaUrl = mediaUrl,
                        MediaType = mediaType,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        CreatedOn = DateTime.Now,
                        ExpiresOn = DateTime.Now.AddHours(24),
                        IsDeleted = false
                    };

                    db.Stories.Add(story);
                    db.SaveChanges();
                }

                response.success = true;
                response.msg = "Story uploaded successfully.";
            }
            catch (Exception ex)
            {
                response.success = false;
                response.msg = "Error: " + ex.Message;
            }

            return response;
        }

        public List<StoryDTO> GetAllUserStories(int userId)
        {
            var query = (
                from s in _db.Stories
                join u in _db.TblUser on s.UserId equals u.Id
                join f in _db.Friendships
                    on s.UserId equals (f.RequesterId == userId ? f.AddresseeId : f.RequesterId)
                    into friendJoin
                from f in friendJoin.DefaultIfEmpty()
                where
                    s.IsDeleted == false &&
                    s.ExpiresOn > DateTime.Now &&
                    (
                        s.UserId == userId || // show user’s own stories
                        (
                            f != null &&
                            f.Status == "A" &&
                            (f.RequesterId == userId || f.AddresseeId == userId)
                        )
                    )
                select new
                {
                    s.UserId,
                    s.MediaUrl,
                    s.MediaType,
                    s.CreatedOn,
                    u.FirstName,
                    u.LastName,
                    u.ProfilePhoto
                }
            ).ToList();

            var result = query
                .GroupBy(x => new { x.UserId, x.FirstName, x.LastName, x.ProfilePhoto })
                .Select(g =>
                {
                    var latest = g.OrderByDescending(x => x.CreatedOn).First();
                    return new StoryDTO
                    {
                        UserId = g.Key.UserId,
                        UserName = g.Key.FirstName + " " + g.Key.LastName,
                        ProfilePic = string.IsNullOrEmpty(g.Key.ProfilePhoto)
                            ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                            : g.Key.ProfilePhoto,
                        MediaUrl = latest.MediaUrl,
                        MediaType = latest.MediaType
                    };
                })
                .ToList();

            return result;
        }



        public List<StoryDTO> GetStoriesByUser(int userId, int loggedInUserId)
        {
            var userStories = (from s in _db.Stories
                               join u in _db.TblUser on s.UserId equals u.Id
                               where s.IsDeleted == false
                                     && s.ExpiresOn > DateTime.Now
                                     && s.UserId == userId
                               orderby s.CreatedOn descending
                               select new StoryDTO
                               {
                                   UserId = u.Id,
                                   StoryId = (int)s.StoryId,
                                   UserName = u.FirstName ?? "" + " " + u.LastName ?? "",
                                   ProfilePic = u.ProfilePhoto,
                                   MediaUrl = s.MediaUrl,
                                   MediaType = s.MediaType,
                                   IsLikedByCurrentUser= _db.TblStoryLike.Any(l => l.StoryId == s.StoryId && l.LikedBy == loggedInUserId)
                               })
                               .ToList();

            return userStories;
        }

        public List<StoryDTO> GetStoriesBStoryId(int StoryId)
        {
            var userStories = (from s in _db.Stories
                               join u in _db.TblUser on s.UserId equals u.Id
                               where s.IsDeleted == false
                                     && s.ExpiresOn > DateTime.Now
                                     && s.StoryId == StoryId
                               orderby s.CreatedOn descending
                               select new StoryDTO
                               {
                                   UserId = u.Id,
                                   StoryId = (int)s.StoryId,
                                   UserName = u.FirstName ?? "" + " " + u.LastName ?? "",
                                   ProfilePic = u.ProfilePhoto,
                                   MediaUrl = s.MediaUrl,
                                   MediaType = s.MediaType,
                                   IsLikedByCurrentUser = _db.TblStoryLike.Any(l => l.StoryId == s.StoryId)
                               })
                               .ToList();

            return userStories;
        }

        public CommonResponse2 AddLike(long postId, int UserId, string ReactionType)
        {
            try
            {
                CommonResponse2 res = new CommonResponse2();
                using (var db = new AppDbContext())
                {

                    var existing = _db.TblPostLike.FirstOrDefault(l => l.PostId == postId && l.UserId == UserId);

                    if (existing == null)
                    {
                        _db.TblPostLike.Add(new TblPostLike
                        {
                            PostId = postId,
                            UserId = UserId,
                            ReactionType = ReactionType,
                            CreatedOn = DateTime.Now,
                        });
                    }
                    else
                    {
                        existing.ReactionType = ReactionType;
                        existing.CreatedOn = DateTime.Now;
                    }


                    _db.SaveChanges();

                    var counts = _db.TblPostLike
                    .Where(l => l.PostId == postId)
                    .GroupBy(l => l.ReactionType)
                    .Select(g => new
                    {
                        ReactionType = g.Key,
                        Count = g.Count()
                    })
                    .ToList();



                    var post = _db.Posts.FirstOrDefault(p => p.PostId == postId);
                    var user = _db.TblUser.FirstOrDefault(p => p.Id == UserId);

                    string emoji = "👍";

                    if (ReactionType == "Love")
                        emoji = "❤️";
                    else if (ReactionType == "Sad")
                        emoji = "😢";
                    else if (ReactionType == "Angry")
                        emoji = "😡";
                    else if (ReactionType == "Wow")
                        emoji = "😮";
                    else if (ReactionType == "Haha")
                        emoji = "😂";
                    else if (ReactionType == "Care")
                        emoji = "🤗";

                    if (UserId != post.UserId)
                    {
                        var notification = new Notifications
                        {
                            PostId = (int)postId,
                            Message = $"{user.FirstName} reacted {emoji} to your post.",
                            UserId = post.UserId,
                            ActorId = UserId,
                            NotificationType = "PostLike",
                            EntityType = "PostLike",
                            EntityId = post.PostId,
                            IsRead = false,
                            CreatedOn = DateTime.UtcNow
                        };

                        _db.Notifications.Add(notification);
                        _db.SaveChanges();
                    }
                   

                    res.success = false;
                    res.data = counts;
                    res.msg = $"{user.FirstName} reacted '{ReactionType}' to your post.";
                    res.UserId = post.UserId;
                    res.id = 0;
                    return res;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public CommonResponse2 AddCommentLike(long commentId, int userId)
        {
            using (var db = new AppDbContext())
            {
                CommonResponse2 res = new CommonResponse2();

                // Check if the user already liked this comment/reply
                var existing = db.TblPostLike.FirstOrDefault(l => l.CommentId == commentId && l.UserId == userId);

                if (existing == null)
                {
                    // Add new like
                    db.TblPostLike.Add(new TblPostLike
                    {
                        CommentId = commentId,
                        UserId = userId,
                        ReactionType = "Love",
                        CreatedOn = DateTime.Now
                    });
                    res.msg = "liked your comment ❤️";
                }
                else
                {
                    // Remove like (toggle)
                    db.TblPostLike.Remove(existing);
                    res.msg = "removed like from your comment 💔";
                }

                db.SaveChanges();

                // Count likes
                var likeCount = db.TblPostLike.Count(x => x.CommentId == commentId);

                // Fetch comment owner
                var comment = db.Comments.FirstOrDefault(c => c.CommentId == commentId);
                var user = db.TblUser.FirstOrDefault(u => u.Id == userId);

                if (comment != null && user != null)
                {
                    // Create notification
                    if (userId != comment.UserId)
                    {
                        var notification = new Notifications
                        {
                            PostId = (int?)comment.PostId,
                            Message = $"{user.FirstName} {res.msg}",
                            UserId = comment.UserId,
                            ActorId = userId,
                            NotificationType = "CommentLike",
                            EntityType = "CommentLike",
                            EntityId = commentId,
                            IsRead = false,
                            CreatedOn = DateTime.UtcNow
                        };

                        db.Notifications.Add(notification);
                        db.SaveChanges();
                    }

                    res.UserId = comment.UserId;
                }

                res.success = true;
                res.data = likeCount;
                res.id = (int?)commentId;
                return res;
            }
        }

        public CommonResponse2 StoryLike(int StoryId, int userId)
        {
            using (var db = new AppDbContext())
            {
                CommonResponse2 res = new CommonResponse2();
                var StoryDet = db.Stories.Where(id => id.StoryId == StoryId && id.IsDeleted == false).FirstOrDefault();
                // Check if the user already liked this comment/reply
                var existing = db.TblStoryLike.FirstOrDefault(l => l.StoryId == StoryId && l.LikedBy == userId);

                if (existing == null)
                {
                    // Add new like
                    db.TblStoryLike.Add(new TblStoryLike
                    {
                        StoryId = StoryId,
                        CreatedOn = DateTime.Now,
                        ReactionType = "Love",
                        LikedBy = userId,
                        StoryCreatedBy= StoryDet.UserId,
                        IsDeleted = false
                    });
                    res.msg = "liked your Story ❤️";
                }
                else
                {
                    // Remove like (toggle)
                    db.TblStoryLike.Remove(existing);
                    res.msg = "removed like from your story 💔";
                }

                var view = db.TblStoryView.FirstOrDefault(v => v.StoryId == StoryId && v.ViewedBy == userId);
                if (view != null)
                {
                    view.IsLiked = existing == null ? true : false;
                    db.TblStoryView.Update(view);
                    db.SaveChanges();
                }

                db.SaveChanges();

                // Count likes
                var likeCount = db.TblStoryLike.Count(x => x.StoryId == StoryId);

              
                var user = db.TblUser.FirstOrDefault(u => u.Id == userId);

                if (StoryDet != null && user != null)
                {
                    // Create notification
                    if (userId != StoryDet.UserId)
                    {
                        var notification = new Notifications
                        {
                            PostId = (int?)StoryDet.StoryId,
                            Message = $"{user.FirstName} {res.msg}",
                            UserId = StoryDet.UserId,
                            ActorId = userId,
                            NotificationType = "StoryLike",
                            EntityType = "TblStoryLike",
                            EntityId = StoryId,
                            IsRead = false,
                            CreatedOn = DateTime.Now
                        };

                        db.Notifications.Add(notification);
                        db.SaveChanges();
                    }

                    res.UserId = StoryDet.UserId;
                }

                res.success = true;
                res.data = likeCount;
                res.id = (int?)StoryId;
                return res;
            }
        }

    }
}
