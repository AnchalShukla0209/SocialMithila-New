using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class RateLimits
    {
        [Key]
        public int RateLimitId { get; set; }
        public int? UserId { get; set; }
        [StringLength(50)]
        public string ActionType { get; set; }
        public int? Count { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? WindowStart { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? WindowEnd { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.RateLimits))]
        public virtual TblUser User { get; set; }
    }
}
