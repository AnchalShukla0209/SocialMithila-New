using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.EntityFrameworkCore;
using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.DataAccess.ResponseModel.UserProfile;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialMithila.Business.Business
{
    public class BllUserProfile : IBllUserProfile
    {

     
        private readonly AppDbContext _db;
        private readonly WebPushService _push;

        public BllUserProfile(WebPushService push)
        {
            _db = new AppDbContext();
            _push = push;
        }

        public IEnumerable<PostFeedDTOs> GetFeed(int userId, int skip, int take, int id)
        {
            IQueryable<int> targetUserIds;

            if (id == 0)
            {
                
                var friends1 = _db.Friendships
                    .Where(f => f.RequesterId == userId && f.Status == "A")
                    .Select(f => f.AddresseeId);

                // Friends where current user is addressee
                var friends2 = _db.Friendships
                    .Where(f => f.AddresseeId == userId && f.Status == "A")
                    .Select(f => f.RequesterId);

                // Combine both sides + current user
                targetUserIds = friends1
                    .Union(friends2)
                    .Union(_db.TblUser.Where(u => u.Id == userId).Select(u => u.Id))
                    .Distinct();
            }
            else
            {
                // 🧩 Case 2: Profile Feed → Posts by profile user + liked/commented posts by that user

                var likedPostIds = _db.TblPostLike
                    .Where(l => l.UserId == id && l.PostId > 0)
                    .Select(l => l.PostId);

                var commentedPostIds = _db.Comments
                    .Where(c => c.UserId == id && c.IsDeleted != true)
                    .Select(c => c.PostId);

                var interactedPostIds = likedPostIds
                    .Union(commentedPostIds)
                    .Distinct();

                // Limit posts to those created by that user or they interacted with
                var feedQuery = from post in _db.Posts
                                join user in _db.TblUser on post.UserId equals user.Id
                                where user.IsActive==true && user.IsDeleted == false
                                      && (user.Id == id || interactedPostIds.Contains(post.PostId))
                                orderby post.PostId descending
                                select new { post, user };

                return feedQuery
                    .Skip(skip)
                    .Take(take)
                    .Select(x => new PostFeedDTOs
                    {
                        PostId = x.post.PostId,
                        UserId = x.user.Id,
                        UserName = x.user.FirstName + " " + (x.user.LastName ?? ""),
                        ProfilePhoto = x.user.ProfilePhoto,
                        Description = x.post.ContentText,
                        MediaFiles = _db.PostMedia
                            .Where(pm => pm.PostId == x.post.PostId)
                            .Select(pm => new MediaDTO
                            {
                                Url = pm.MediaUrl,
                                Type = pm.MediaType
                            }).ToList(),
                        CreatedOn = x.post.CreatedOn ?? DateTime.Now,
                        LikeCount = _db.TblPostLike.Count(l => l.PostId == x.post.PostId),
                        CommentCount = _db.Comments.Count(c => c.PostId == x.post.PostId),
                        UserReaction = _db.TblPostLike
                            .Where(l => l.PostId == x.post.PostId && l.UserId == userId)
                            .Select(l => l.ReactionType)
                            .FirstOrDefault(),
                        UserReactions = _db.TblPostLike
                            .Where(l => l.PostId == x.post.PostId)
                            .Include(l => l.User)
                            .Select(l => new UserReactionDTO
                            {
                                UserName = l.User.FirstName + " " + (l.User.LastName ?? ""),
                                ReactionType = l.ReactionType
                            }).ToList()
                    })
                    .AsNoTracking()
                    .ToList();
            }

            // 🧩 Case 1 (continued): build home feed from user + friends
            var homeFeed = (from post in _db.Posts
                            join user in _db.TblUser on post.UserId equals user.Id
                            where user.IsActive==true && user.IsDeleted==false
                                  && targetUserIds.Contains(user.Id)
                            orderby post.PostId descending
                            select new { post, user })
                            .Skip(skip)
                            .Take(take)
                            .Select(x => new PostFeedDTOs
                            {
                                PostId = x.post.PostId,
                                UserId = x.user.Id,
                                UserName = x.user.FirstName + " " + (x.user.LastName ?? ""),
                                ProfilePhoto = x.user.ProfilePhoto,
                                Description = x.post.ContentText,
                                MediaFiles = _db.PostMedia
                                    .Where(pm => pm.PostId == x.post.PostId)
                                    .Select(pm => new MediaDTO
                                    {
                                        Url = pm.MediaUrl,
                                        Type = pm.MediaType
                                    }).ToList(),
                                CreatedOn = x.post.CreatedOn ?? DateTime.Now,
                                LikeCount = _db.TblPostLike.Count(l => l.PostId == x.post.PostId),
                                CommentCount = _db.Comments.Count(c => c.PostId == x.post.PostId),
                                UserReaction = _db.TblPostLike
                                    .Where(l => l.PostId == x.post.PostId && l.UserId == userId)
                                    .Select(l => l.ReactionType)
                                    .FirstOrDefault(),
                                UserReactions = _db.TblPostLike
                                    .Where(l => l.PostId == x.post.PostId)
                                    .Include(l => l.User)
                                    .Select(l => new UserReactionDTO
                                    {
                                        UserName = l.User.FirstName + " " + (l.User.LastName ?? ""),
                                        ReactionType = l.ReactionType
                                    }).ToList()
                            })
                            .AsNoTracking()
                            .ToList();

            return homeFeed;
        }




        public PostFeedDTOs GetFeedById(int postId, int userId)
        {
            var feed = (from post in _db.Posts
                        join user in _db.TblUser on post.UserId equals user.Id
                        where user.IsActive == true && user.IsDeleted == false && post.PostId == postId
                        orderby post.PostId descending
                        select new PostFeedDTOs
                        {
                            PostId = post.PostId,
                            UserId = user.Id,
                            UserName = user.FirstName + " " + (user.LastName ?? ""),
                            ProfilePhoto = user.ProfilePhoto,
                            Description = post.ContentText,
                            MediaFiles = _db.PostMedia
                                .Where(pm => pm.PostId == post.PostId)
                                .Select(pm => new MediaDTO
                                {
                                    Url = pm.MediaUrl,
                                    Type = pm.MediaType
                                }).ToList(),
                            CreatedOn = post.CreatedOn ?? DateTime.Now,
                            LikeCount = _db.TblPostLike.Count(l => l.PostId == post.PostId),
                            CommentCount = _db.Comments.Count(c => c.PostId == post.PostId),

                            UserReaction = _db.TblPostLike
                                .Where(l => l.PostId == post.PostId && l.UserId == userId)
                                .Select(l => l.ReactionType)
                                .FirstOrDefault(),

                            UserReactions = _db.TblPostLike
                                .Where(l => l.PostId == post.PostId).Include(l => l.User)
                                .Select(l => new UserReactionDTO
                                {
                                    UserName = l.User.FirstName + " " + (l.User.LastName ?? ""),
                                    ReactionType = l.ReactionType
                                }).ToList()
                        })
            .FirstOrDefault();

            return feed;
        }


        public IEnumerable<PostFeedDTOs> GetPosts(int userId, int skip, int take)
        {
            var feed = (from post in _db.Posts
                        join user in _db.TblUser on post.UserId equals user.Id
                        where user.IsActive ==true && user.IsDeleted == false && post.IsDeleted==false
                        orderby post.CreatedOn descending
                        select new PostFeedDTOs
                        {
                            PostId = post.PostId,
                            UserId = user.Id,
                            UserName = user.FirstName + " " + user.LastName,
                            ProfilePhoto = user.ProfilePhoto,
                            Description = post.ContentText,
                            CreatedOn = (DateTime)post.CreatedOn,
                            MediaFiles = _db.PostMedia
                                .Where(m => m.PostId == post.PostId)
                                .Select(x => new MediaDTO
                                {
                                    Url = x.MediaUrl,
                                    Type = x.MediaType
                                }).ToList(),
                            LikeCount = _db.Reactions.Count(r => r.PostId == post.PostId),
                            CommentCount = _db.Comments.Count(c => c.PostId == post.PostId)
                        })
                        .AsNoTracking()
                        .Skip(skip)
                        .Take(take)
                        .ToList();

            return feed;
        }

        public TblUser GetUser(int userId)
        {
            var user = _db.TblUser.FirstOrDefault(u => u.Id == userId && u.IsDeleted == false);

            return user;
        }





        public IEnumerable<TblUser> SearchUsers(string term, int limit = 10)
        {
            return _db.TblUser
                .Where(u => u.IsDeleted == false && (u.FirstName).Contains(term))
                .Take(limit)
                .ToList();
        }

        public Posts AddPost(Posts post)
        {
            _db.Posts.Add(post);
            return post;
        }

        public void AddPostMedia(IEnumerable<PostMedia> medias)
        {
            _db.PostMedia.AddRange(medias);
        }

        public void AddPostTags(IEnumerable<PostTags> tags)
        {
            _db.PostTags.AddRange(tags);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public PostDto CreatePost(int userId, string contentText, HttpFileCollectionBase files, List<int> mentionedUserIds)
        {
            var uploadedFilePaths = new List<string>();

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var post = new Posts
                    {
                        UserId = userId,
                        ContentText = contentText,
                        CreatedOn = DateTime.UtcNow,
                        Privacy = 0,
                        IsDeleted = false
                    };

                    AddPost(post);
                    SaveChanges();

                    var medias = new List<PostMedia>();

                    if (files != null && files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            var f = files[i];
                            if (f != null && f.ContentLength > 0)
                            {
                                string extension = Path.GetExtension(f.FileName).ToLower();
                                var typeFolder = "";

                                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                                    typeFolder = "Image";
                                else if (extension == ".mp4" || extension == ".mov" || extension == ".avi" || extension == ".mkv")
                                    typeFolder = "Video";

                                var userFolder = HttpContext.Current.Server.MapPath(
                                    $"~/Content/UploadFolder/Posts/{typeFolder}/{userId}");

                                if (!Directory.Exists(userFolder))
                                    Directory.CreateDirectory(userFolder);

                                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(f.FileName)}";
                                var filePath = Path.Combine(userFolder, fileName);

                                f.SaveAs(filePath);
                                uploadedFilePaths.Add(filePath);

                                medias.Add(new PostMedia
                                {
                                    PostId = post.PostId,
                                    MediaUrl = $"/Content/UploadFolder/Posts/{typeFolder}/{userId}/{fileName}",
                                    MediaType = typeFolder,
                                    SortOrder = 0
                                });
                            }
                        }

                        if (medias.Any())
                            AddPostMedia(medias);
                    }

                    if (mentionedUserIds != null && mentionedUserIds.Any())
                    {
                        var tags = mentionedUserIds.Select(id => new PostTags
                        {
                            PostId = post.PostId,
                            HashtagId = id
                        });

                        AddPostTags(tags);
                    }

                    SaveChanges();
                    transaction.Commit();

                    // Map to strongly typed DTO
                    var postDto = new PostDto
                    {
                        PostId = (int)post.PostId,
                        UserId = post.UserId,
                        ContentText = post.ContentText,
                        CreatedOn = (DateTime)post.CreatedOn,
                        Media = medias.Select(m => new PostMediaDto
                        {
                            MediaUrl = m.MediaUrl,
                            MediaType = m.MediaType
                        }).ToList()
                    };

                    // Send via SignalR
                    var hub = GlobalHost.ConnectionManager.GetHubContext<PostHub>();
                    hub.Clients.All.receivePost(postDto);

                    return postDto;
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    foreach (var path in uploadedFilePaths)
                    {
                        if (File.Exists(path))
                        {
                            try { File.Delete(path); }
                            catch { }
                        }
                    }

                    throw;
                }
            }
        }


        public UserProfileViewModel GetUserProfileById(int userId, int currentUserId)
        {
            try
            {
                var query =
                    from u in _db.TblUser
                    where u.Id == userId && u.IsActive == true && u.IsDeleted == false
                    join a in _db.TblAddress.Where(x => x.IsActive == true && x.IsDeleted == false)
                        on u.Id equals a.UserId into addr
                    from a in addr.DefaultIfEmpty()
                    join s in _db.TblSocial.Where(x => x.IsActive == true && x.IsDeleted == false)
                        on u.Id equals s.UserId into social
                    from s in social.DefaultIfEmpty()
                    join p in _db.TblUserPaymentCard.Where(x => x.IsActive == true && x.IsDeleted == false)
                        on u.Id equals p.UserId into card
                    from p in card.DefaultIfEmpty()
                        
                    join f in _db.Friendships
                        .Where(x =>
                            (x.RequesterId == currentUserId && x.AddresseeId == userId) ||
                            (x.RequesterId == userId && x.AddresseeId == currentUserId))
                        on 1 equals 1 into friend
                    from f in friend.DefaultIfEmpty()
                    select new UserProfileViewModel
                    {
                        User = u,
                        Address = a,
                        Social = s,
                        PaymentCard = p,
                        FriendshipStatus = f.Status,
                        FriendshipId = (int?)f.FriendshipId,
                        IsRequester = f != null && f.RequesterId == currentUserId,
                        UserId = u.Id
                    };

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading user profile: " + ex.Message, ex);
            }
        }


        public PostCommentResponseDTO GetCommentsByPostId(long postId, int currentUserId)
        {
            var post = (
                from p in _db.Posts
                join u in _db.TblUser on p.UserId equals u.Id
                where p.PostId == postId
                select new
                {
                    p.PostId,
                    PosterId = u.Id,
                    PosterName = u.FirstName + " " + (u.LastName ?? ""),
                    PosterProfile = string.IsNullOrEmpty(u.ProfilePhoto)
                        ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                        : u.ProfilePhoto
                }
            ).FirstOrDefault();

            if (post == null)
                return null;

            // Local recursive function to get sub replies
            List<CommentDTO> GetSubReplies(long parentCommentId)
            {
                var replies = (
                    from r in _db.Comments
                    join ur in _db.TblUser on r.UserId equals ur.Id
                    where r.ParentCommentId == parentCommentId && r.IsDeleted == false
                    orderby r.CreatedOn
                    select new CommentDTO
                    {
                        CommentId = r.CommentId,
                        PostId = r.PostId,
                        UserId = r.UserId,
                        UserName = ur.FirstName + " " + (ur.LastName ?? ""),
                        ProfilePhoto = string.IsNullOrEmpty(ur.ProfilePhoto)
                            ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                            : ur.ProfilePhoto,
                        CommentText = r.CommentText,
                        CreatedOn = (DateTime)r.CreatedOn,
                        CommentLikeCount = _db.TblPostLike.Count(l => l.CommentId == r.CommentId),
                        CommentReplyCount = _db.Comments.Count(rr => rr.ParentCommentId == r.CommentId && rr.IsDeleted == false),
                        SubReplyCount = _db.Comments.Count(rr => rr.ParentCommentId == r.CommentId && rr.IsDeleted == false),
                        IsLikedByCurrentUser = _db.TblPostLike.Any(l => l.CommentId == r.CommentId && l.UserId == currentUserId)
                    }
                ).ToList();

                // recursion for nested replies
                foreach (var reply in replies)
                {
                    reply.SubReply = GetSubReplies(reply.CommentId);
                }

                return replies;
            }

            // Top-level comments
            var comments = (
                from c in _db.Comments
                join u in _db.TblUser on c.UserId equals u.Id
                where c.PostId == postId && c.IsDeleted == false && (c.ParentCommentId == null || c.ParentCommentId == 0)
                orderby c.CreatedOn ascending
                select new CommentDTO
                {
                    CommentId = c.CommentId,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    UserName = u.FirstName + " " + (u.LastName ?? ""),
                    ProfilePhoto = string.IsNullOrEmpty(u.ProfilePhoto)
                        ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                        : u.ProfilePhoto,
                    CommentText = c.CommentText,
                    CreatedOn = (DateTime)c.CreatedOn,
                    CommentLikeCount = _db.TblPostLike.Count(l => l.CommentId == c.CommentId),
                    CommentReplyCount = _db.Comments.Count(r => r.ParentCommentId == c.CommentId && r.IsDeleted == false),
                    SubReplyCount = _db.Comments.Count(r => r.ParentCommentId == c.CommentId && r.IsDeleted == false),
                    IsLikedByCurrentUser= _db.TblPostLike.Any(l => l.CommentId == c.CommentId && l.UserId == currentUserId)
                }
            ).AsNoTracking().ToList();

            // Fetch sub replies recursively
            foreach (var comment in comments)
            {
                comment.SubReply = GetSubReplies(comment.CommentId);
            }

            var response = new PostCommentResponseDTO
            {
                PostId = post.PostId,
                PosterId = post.PosterId,
                PosterName = post.PosterName,
                PosterProfile = post.PosterProfile,
                TotalPostCommentCount = _db.Comments.Count(c => c.PostId == postId && c.IsDeleted == false),
                TotalPostLikeCount = _db.TblPostLike.Count(l => l.PostId == postId),
                Comments = comments
            };

            return response;
        }






        public ResponseModel AddComment(CommentsRequestModel model)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    if (string.IsNullOrWhiteSpace(model.CommentText))
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Comment text cannot be empty."
                        };
                    }

                    int parentId = (int)(model.ParentCommentId ?? 0);

                    // --- 1️⃣ Save Comment ---
                    var entity = new Comments
                    {
                        PostId = model.PostId,
                        UserId = (int)model.UserId,
                        ParentCommentId = parentId,
                        CommentText = model.CommentText.Trim(),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };

                    db.Comments.Add(entity);
                    db.SaveChanges();

                    // --- 2️⃣ Determine Notification Receiver ---
                    int? receiverUserId = null;
                    string message = string.Empty;
                    string notificationType = string.Empty;
                    string entityType = string.Empty;

                    if (parentId == 0)
                    {
                        // Comment on post
                        var post = db.Posts.FirstOrDefault(p => p.PostId == model.PostId);
                        if (post != null && post.UserId != model.UserId)
                        {
                            receiverUserId = post.UserId;
                            notificationType = "Comment";
                            entityType = "Post";
                            message = "commented on your post.";
                        }
                    }
                    else
                    {
                        // Reply to another comment
                        var parentComment = db.Comments.FirstOrDefault(c => c.CommentId == parentId);
                        if (parentComment != null && parentComment.UserId != model.UserId)
                        {
                            receiverUserId = parentComment.UserId;
                            notificationType = "Reply";
                            entityType = "Comment";
                            message = "replied to your comment.";
                        }
                    }

                    // --- 3️⃣ Save Notification ---
                    if (receiverUserId.HasValue)
                    {
                        var notification = new Notifications
                        {
                            UserId = receiverUserId.Value,
                            ActorId = (int)model.UserId,
                            NotificationType = notificationType,
                            EntityType = entityType,
                            EntityId = entity.CommentId,
                            IsRead = false,
                            CreatedOn = DateTime.Now,
                            PostId = (int?)model.PostId,
                            Message = message
                        };

                        db.Notifications.Add(notification);
                        db.SaveChanges();

                        // --- 4️⃣ Send Notification in Real-Time via SignalR ---
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                        var actor = db.TblUser.Find(model.UserId);

                        hubContext.Clients.Group(receiverUserId.ToString()).receiveNotification(new
                        {
                            FromUserName = actor.FirstName + " " + actor.LastName,
                            Message = $"{actor.FirstName} {actor.LastName} {message}",
                            ProfilePhoto = string.IsNullOrEmpty(actor.ProfilePhoto)? "https://cdn-icons-png.flaticon.com/512/149/149071.png":actor.ProfilePhoto,
                            CreatedOn = DateTime.Now.ToString("g")
                        });
                    }

                    // --- 5️⃣ Send Real-Time Comment Update to Post Viewers ---
                    var commentHub = GlobalHost.ConnectionManager.GetHubContext<CommentHub>();
                    var user = db.TblUser.Find(model.UserId);

                    commentHub.Clients.Group(model.PostId.ToString()).newCommentAdded(new
                    {
                        CommentId = entity.CommentId,
                        PostId = entity.PostId,
                        UserId = entity.UserId,
                        CommentText = entity.CommentText,
                        CreatedOn = entity.CreatedOn?.ToString("g"),
                        ParentCommentId = entity.ParentCommentId,
                        UserName = user.FirstName + " " + user.LastName,
                        ProfilePhoto = string.IsNullOrEmpty(user.ProfilePhoto)? "https://cdn-icons-png.flaticon.com/512/149/149071.png" : user.ProfilePhoto
                    });

                    // --- 6️⃣ Return Result ---
                    string msg = parentId == 0
                        ? "Comment added successfully on post."
                        : "Reply added successfully on comment.";

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Message = msg
                    };
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


       

        public ResponseModel SocialAccount(TblSocialmodel model)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    
                    if (model.UserId == 0)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "User ID is required."
                        };
                    }
                    // 🔹 Check if social data already exists for this user
                    var existing = db.TblSocial.FirstOrDefault(s => s.UserId == model.UserId && s.IsDeleted==false);

                    if (existing != null)
                    {
                        // --- Update existing record ---
                        existing.FacebookProfile = model.FacebookProfile?.Trim();
                        existing.Twitter = model.Twitter?.Trim();
                        existing.LinkedIn = model.LinkedIn?.Trim();
                        existing.Instagram = model.Instagram?.Trim();
                        existing.Flickr = model.Flickr?.Trim();
                        existing.Github = model.Github?.Trim();
                        existing.Skype = model.Skype?.Trim();
                        existing.Google = model.Google?.Trim();
                        existing.IsActive = true;
                        existing.ModifiedOn = DateTime.Now;
                        existing.ModifiedBy = Convert.ToString(model.ModifiedBy);

                    }
                    else
                    {
                        // --- Insert new record ---
                        var entity = new TblSocial
                        {
                            UserId = model.UserId,
                            FacebookProfile = model.FacebookProfile?.Trim(),
                            Twitter = model.Twitter?.Trim(),
                            LinkedIn = model.LinkedIn?.Trim(),
                            Instagram = model.Instagram?.Trim(),
                            Flickr = model.Flickr?.Trim(),
                            Github = model.Github?.Trim(),
                            Skype = model.Skype?.Trim(),
                            Google = model.Google?.Trim(),
                            IsActive = true,
                            IsDeleted = false,
                            CreatedOn = DateTime.Now,
                            CreatedBy = Convert.ToString(model.CreatedBy)
                        };

                        db.TblSocial.Add(entity);
                    }

                    db.SaveChanges();

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Message = "Social account information saved successfully."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Error: " + ex.Message
                };
            }
        }

        public ResponseModel ChangePassword(TblPasswordChange model)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    // Step 1: Validate
                    if (model.UserId == 0)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "User not found. Please login again."
                        };
                    }

                    if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Please fill all required fields."
                        };
                    }

                    // Step 2: Find user record
                    var existing = db.TbleUserPassword.FirstOrDefault(x => x.UserId == model.UserId);

                    if (existing == null)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "User record not found."
                        };
                    }

                    // Step 3: Check old password match
                    if (existing.NewPassword != model.OldPassword)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Old password is incorrect."
                        };
                    }

                    // Step 4: Check new password & confirm password match
                    if (model.NewPassword != model.NewPassword)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "New and Confirm passwords do not match."
                        };
                    }

                    // Step 5: Update password
                    existing.OldPassword = existing.NewPassword; // keep history
                    existing.NewPassword = model.NewPassword.Trim();
                    existing.ModifiedOn = DateTime.Now;

                    db.SaveChanges();

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Message = "Password changed successfully."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Error: " + ex.Message
                };
            }
        }





    }
}
