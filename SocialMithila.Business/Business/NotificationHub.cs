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
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {
            var userId = Context.QueryString["userId"];
            userId = string.IsNullOrEmpty(userId) ? Context.User.Identity.Name : userId;
            if (!string.IsNullOrEmpty(userId))
            {
               
                Groups.Add(Context.ConnectionId, userId);
                System.Diagnostics.Debug.WriteLine($"NotificationHub User {userId} joined FriendRequestHub group {userId}");
            }
            return base.OnConnected();
        }


        public void SendNotification(string toUserId, string message)
        {
            Console.WriteLine("NotificationHub: SendNotification() called at " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine("NotificationHub: SendNotification() called at " + DateTime.Now);
            Clients.Group(toUserId).receiveNotification(message);
            Console.WriteLine("NotificationHub: SendNotification() sent  at " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine("NotificationHub: SendNotification() sent at " + DateTime.Now);
        }
    }
}
