using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel.UserProfile
{
    public class UserProfileViewModel
    {
        public TblUser User { get; set; }
        public TblAddress Address { get; set; }
        public TblSocial Social { get; set; }
        public TblUserPaymentCard PaymentCard { get; set; }
        public string FriendshipStatus { get; set; }
        public int? FriendshipId { get; set; }
        public bool IsRequester { get; set; }
        public int UserId { get; set; }
    }
}
