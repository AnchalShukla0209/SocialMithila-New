using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class MediaFiles
    {
        [Key]
        public long MediaId { get; set; }
        public int? OwnerUserId { get; set; }
        [Required]
        [StringLength(500)]
        public string MediaUrl { get; set; }
        [Required]
        [StringLength(30)]
        public string MediaType { get; set; }
        [StringLength(260)]
        public string FileName { get; set; }
        public long? SizeBytes { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey(nameof(OwnerUserId))]
        [InverseProperty(nameof(TblUser.MediaFiles))]
        public virtual TblUser OwnerUser { get; set; }
    }
}
