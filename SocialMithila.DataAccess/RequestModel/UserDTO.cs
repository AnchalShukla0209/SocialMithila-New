using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.RequestModel
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string FristNAme { get; set; }
        public string LastNAme { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string TownCity { get; set; }
        public string PostCode { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto  { get; set; }
    }

   

    public class GetAccountinformationDetails
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public UserDTO Data { get; set; }
    }

    public class GetfriendRequest
    {
        public int? AddresseeId { get; set; }
        public int? RequesterId { get; set; }
        public int? FriendshipId { get; set; }
        public string FristNAme { get; set; }
        public string LastNAme { get; set; }
        public string ProfilePhoto { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class GetGetfriendRequestDetails
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public List<GetfriendRequest> Data { get; set; }
    }


}
