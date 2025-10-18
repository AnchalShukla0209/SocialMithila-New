using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("business_images")]
    public partial class BusinessImage
    {
        [Key]
        [Column("image_id")]
        public int ImageId { get; set; }

        [Column("business_id")]
        public int BusinessId { get; set; }

        [Column("image_url")]
        [Required, MaxLength(500)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public Business Business { get; set; }
    }
}
