using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPush;

namespace SocialMithila.Business.Business
{
    public class WebPushService
    {
        private readonly string _publicKey;
        private readonly string _privateKey;

        public WebPushService()
        {
            _publicKey = ConfigurationManager.AppSettings["VapidPublicKey"];
            _privateKey = ConfigurationManager.AppSettings["VapidPrivateKey"];
        }

        public void SendNotification(string endpoint, string p256dh, string auth, string message)
        {
            var sub = new PushSubscription(endpoint, p256dh, auth);
            var vapid = new VapidDetails("mailto:anchalshukla7060153412@gmail.com", _publicKey, _privateKey);
            var payload = JsonConvert.SerializeObject(new { title = "New Notification", body = message });

            try
            {
                new WebPushClient().SendNotification(sub, payload, vapid);
            }
            catch { }
        }
    }
}
