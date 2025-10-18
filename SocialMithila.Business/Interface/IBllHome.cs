using Microsoft.AspNetCore.Http;
using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.DataAccess.ResponseModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialMithila.Business.Interface
{
    public interface IBllHome
    {
        CommonResponse Accountinformation(UserDTO user, HttpPostedFileBase profilePhotoFile);
        GetAccountinformationDetails GetAccountinformation(int Id);
        GetGetfriendRequestDetails GetFriendRequest(int Id);
        CommonResponse AddStory(decimal Latitude, decimal Longitude, HttpPostedFileBase profilePhotoFile, int userId);
        List<StoryDTO> GetAllUserStories(int userId);
        List<StoryDTO> GetStoriesByUser(int userId, int loggedInUserId);

        CommonResponse2 AddLike(long postId, int UserId, string ReactionType);
        CommonResponse2 AddCommentLike(long commentId, int userId);
        CommonResponse2 StoryLike(int StoryId, int userId);
        List<StoryDTO> GetStoriesBStoryId(int StoryId);
    }
}
