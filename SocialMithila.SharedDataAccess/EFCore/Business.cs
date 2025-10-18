using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("businesses")]
    public partial class Business
    {
        [Key]
        [Column("business_id")]
        public int BusinessId { get; set; }

        [Column("business_name")]
        [Required, MaxLength(150)]
        public string BusinessName { get; set; }

        [Column("category_id")]
        [Required]
        public int CategoryId { get; set; }

        [Column("sub_category_id")]
        public int? SubCategoryId { get; set; }

        [Column("contact_number")]
        [MaxLength(20)]
        public string ContactNumber { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("latitude", TypeName = "decimal(10,7)")]
        public decimal? Latitude { get; set; }

        [Column("longitude", TypeName = "decimal(10,7)")]
        public decimal? Longitude { get; set; }

        [Column("rating", TypeName = "decimal(2,1)")]
        [Range(0, 5)]
        public decimal? Rating { get; set; } = 0.0m;

        [Column("distance_km", TypeName = "decimal(5,2)")]
        public decimal? DistanceKm { get; set; }

        [Column("image_url")]
        [MaxLength(500)]
        public string ImageUrl { get; set; }  // Optional featured image

        [Column("payment_id")]
        public int? PaymentId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public string Descreption { get; set; }
        public int? userid { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public virtual SubCategory SubCategory { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ICollection<BusinessFeature> BusinessFeatures { get; set; }
        public virtual ICollection<BusinessReview> BusinessReviews { get; set; }
        public virtual ICollection<BusinessImage> BusinessImages { get; set; }

        public Business()
        {
            BusinessFeatures = new HashSet<BusinessFeature>();
            BusinessReviews = new HashSet<BusinessReview>();
            BusinessImages = new HashSet<BusinessImage>();
        }
    }
}
