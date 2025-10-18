using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Stories
    {
        [Key]
        public long StoryId { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(500)]
        public string MediaUrl { get; set; }
        [Required]
        [StringLength(30)]
        public string MediaType { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime ExpiresOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Longitude { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.Stories))]
        public virtual TblUser User { get; set; }
    }
}
