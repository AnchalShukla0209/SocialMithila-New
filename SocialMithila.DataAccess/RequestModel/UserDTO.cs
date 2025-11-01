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



    public class TblSocialmodel
    {
        public int SocialId { get; set; }
        public int UserId { get; set; }
        public string FacebookProfile { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string Flickr { get; set; }
        public string Github { get; set; }
        public string Skype { get; set; }
        public string Google { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class TblPasswordChange
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
