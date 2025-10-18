using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("business_reviews")]
    public partial class BusinessReview
    {
        [Key]
        [Column("review_id")]
        public int ReviewId { get; set; }

        [Column("business_id")]
        public int BusinessId { get; set; }

        [Column("user_name")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Column("rating", TypeName = "decimal(2,1)")]
        public decimal? Rating { get; set; }

        [Column("review_text")]
        public string ReviewText { get; set; }
        public int userid { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public Business Business { get; set; }
    }
}
