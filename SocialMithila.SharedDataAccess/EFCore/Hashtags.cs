using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Hashtags
    {
        public Hashtags()
        {
            PostTags = new HashSet<PostTags>();
        }

        [Key]
        public int HashtagId { get; set; }
        [StringLength(100)]
        public string Tag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [InverseProperty("Hashtag")]
        public virtual ICollection<PostTags> PostTags { get; set; }
    }
}
