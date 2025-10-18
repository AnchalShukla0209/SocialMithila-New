using Microsoft.AspNet.SignalR;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.Business.Business
{
    public class FriendRequestHub : Hub
    {
        public override Task OnConnected()
        {
            var userId = Context.QueryString["userId"];
            userId = string.IsNullOrEmpty(userId) ? Context.User.Identity.Name : userId;
            if (!string.IsNullOrEmpty(userId))
            {

                Groups.Add(Context.ConnectionId, userId);
                System.Diagnostics.Debug.WriteLine($"FriendRequestHub User {userId} joined FriendRequestHub group {userId}");
            }
            return base.OnConnected();
        }


        public void NotifyFriendRequest(int receiverId, int fromUserId, string status)
        {
            // Send info to client
            Clients.Group(receiverId.ToString()).friendRequestUpdated(fromUserId, status);
        }

    }
}
