using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class GroupMembers
    {
        [Key]
        public long GroupMemberId { get; set; }
        public long GroupId { get; set; }
        public int UserId { get; set; }
        public byte Role { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? JoinedOn { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(Groups.GroupMembers))]
        public virtual Groups Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.GroupMembers))]
        public virtual TblUser User { get; set; }
    }
}
