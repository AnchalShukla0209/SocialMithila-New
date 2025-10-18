using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Friendships
    {
        [Key]
        public long FriendshipId { get; set; }
        public int RequesterId { get; set; }
        public int AddresseeId { get; set; }
        public string Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey(nameof(AddresseeId))]
        [InverseProperty(nameof(TblUser.FriendshipsAddressee))]
        public virtual TblUser Addressee { get; set; }
        [ForeignKey(nameof(RequesterId))]
        [InverseProperty(nameof(TblUser.FriendshipsRequester))]
        public virtual TblUser Requester { get; set; }
    }
}
