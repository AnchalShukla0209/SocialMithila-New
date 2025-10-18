using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class UserFeed
    {
        [Key]
        public long FeedId { get; set; }
        public int UserId { get; set; }
        public long PostId { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        public bool? IsRead { get; set; }
    }
}
