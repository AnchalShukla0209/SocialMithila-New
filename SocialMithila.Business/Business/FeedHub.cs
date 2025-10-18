using Microsoft.AspNet.SignalR;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.Business.Business
{
    using Microsoft.AspNet.SignalR;
    using System;
    using System.ComponentModel.Design;
    using System.Threading.Tasks;

    public class FeedHub : Hub
    {


        public override Task OnConnected()
        {
            var userId = Context.QueryString["userId"];
            userId = string.IsNullOrEmpty(userId) ? Context.User.Identity.Name : userId;

            if (!string.IsNullOrEmpty(userId))
            {
                Groups.Add(Context.ConnectionId, userId);
                System.Diagnostics.Debug.WriteLine($"FeedHub: User {userId} joined group {userId}");
            }

            return base.OnConnected();
        }

        public async Task UpdateReaction(long postId, int LikeCount, List<UserReactionDTO> UserReactions)
        {
            try
            {
                
                await Clients.All.updateReaction(postId, LikeCount, UserReactions);
                System.Diagnostics.Debug.WriteLine($"FeedHub: UpdateReaction triggered for PostId={postId}, UserId={0}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FeedHub: Error in UpdateReaction - {ex.Message}");
            }
        }

        public void UpdateLikeCount(int postId, string ReactionType, string userId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"FeedHub: UpdateLikeCount() called at {DateTime.Now}");
                Clients.Group(userId).updateLikeCount(postId, ReactionType);
                System.Diagnostics.Debug.WriteLine($"FeedHub: UpdateLikeCount() sent at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FeedHub: Error in UpdateLikeCount - " + ex.Message);
            }
        }

        public void updateCommentLike(int commentId, int likeCount, string userId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"FeedHub: UpdateLikeCount() called at {DateTime.Now}");
                Clients.Group(userId).updateLikeCount(commentId, likeCount);
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FeedHub: Error in UpdateLikeCount - " + ex.Message);
            }
        }

        public void updateStoryLike(int storyId, int likeCount, string userId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"FeedHub: updateStoryLike() called at {DateTime.Now}");
                Clients.Group(userId).updateStoryLike(storyId, likeCount);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FeedHub: Error in UpdateLikeCount - " + ex.Message);
            }
        }
    }


}
