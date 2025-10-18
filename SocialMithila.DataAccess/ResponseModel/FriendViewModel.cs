using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel
{
    public class FriendViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public int MutualFriends { get; set; }
    }

}
