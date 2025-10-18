using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("user_business_follows")]
    public partial class UserBusinessFollow
    {
        [Key]
        [Column("follow_id")]
        public int FollowId { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Column("business_id")]
        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }

        [Column("follow_date")]
        public DateTime FollowDate { get; set; } = DateTime.UtcNow;

        public virtual TblUser User { get; set; }
        public virtual Business Business { get; set; }
    }
}
