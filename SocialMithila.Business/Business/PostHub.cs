using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.Business.Business
{
    public class PostHub : Hub
    {
        public void SendLikeNotification(int postId, int userId=0, string message="")
        {
            Clients.All.notifyLike(postId, userId, message);
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
        public void BroadcastPost(object postDto)
        {
           
            Clients.All.receivePost(postDto);
        }
    }
}
