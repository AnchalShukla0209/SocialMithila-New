using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class PostMedia
    {
        [Key]
        public long PostMediaId { get; set; }
        public long PostId { get; set; }
        [Required]
        [StringLength(500)]
        public string MediaUrl { get; set; }
        [Required]
        [StringLength(30)]
        public string MediaType { get; set; }
        public byte? SortOrder { get; set; }
        public string MetadataJson { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty(nameof(Posts.PostMedia))]
        public virtual Posts Post { get; set; }
    }
}
