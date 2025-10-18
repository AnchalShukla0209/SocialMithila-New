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
    public class CommentHub : Hub
    {
        public override Task OnConnected()
        {
            var userId = Context.QueryString["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                Groups.Add(Context.ConnectionId, userId);
                System.Diagnostics.Debug.WriteLine($"✅ User {userId} joined their personal CommentHub group");
            }
            return base.OnConnected();
        }

        // 👇 Add this
        public Task JoinPostGroup(string postId)
        {
            System.Diagnostics.Debug.WriteLine($"👥 Connection {Context.ConnectionId} joined post group {postId}");
            return Groups.Add(Context.ConnectionId, postId);
        }

        // 👇 Optional — when user closes popup/lightbox
        public Task LeavePostGroup(string postId)
        {
            System.Diagnostics.Debug.WriteLine($"🚪 Connection {Context.ConnectionId} left post group {postId}");
            return Groups.Remove(Context.ConnectionId, postId);
        }
    }

}
