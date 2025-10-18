using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class PostTags
    {
        [Key]
        public int PostTagId { get; set; }
        public long? PostId { get; set; }
        public int? HashtagId { get; set; }

        [ForeignKey(nameof(HashtagId))]
        [InverseProperty(nameof(Hashtags.PostTags))]
        public virtual Hashtags Hashtag { get; set; }
        [ForeignKey(nameof(PostId))]
        [InverseProperty(nameof(Posts.PostTags))]
        public virtual Posts Post { get; set; }
    }
}
