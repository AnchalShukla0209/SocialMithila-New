using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.DataAccess.ResponseModel.UserProfile;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialMithila.Business.Interface
{
    public interface IBllUserProfile
    {
        IEnumerable<PostFeedDTOs> GetFeed(int userId, int skip, int take, int id);
        TblUser GetUser(int userId);
        IEnumerable<TblUser> SearchUsers(string term, int limit = 10);
        Posts AddPost(Posts post);
        void AddPostMedia(IEnumerable<PostMedia> medias);
        void AddPostTags(IEnumerable<PostTags> tags);
        void SaveChanges();
        PostDto CreatePost(int userId, string contentText, HttpFileCollectionBase files, List<int> mentionedUserIds);
        UserProfileViewModel GetUserProfileById(int userId, int currentUserId);
        PostCommentResponseDTO GetCommentsByPostId(long postId, int currentUserId);

        PostFeedDTOs GetFeedById(int postId, int userId);
        ResponseModel AddComment(CommentsRequestModel model);
    }
}
