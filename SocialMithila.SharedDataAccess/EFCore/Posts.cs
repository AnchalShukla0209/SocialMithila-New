using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Posts
    {
        public Posts()
        {
            Comments = new HashSet<Comments>();
            PostMedia = new HashSet<PostMedia>();
            PostTags = new HashSet<PostTags>();
            Reactions = new HashSet<Reactions>();
            UserMentions = new HashSet<UserMentions>();
        }

        [Key]
        public long PostId { get; set; }
        public int UserId { get; set; }
        public string ContentText { get; set; }
        public byte Privacy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Longitude { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.Posts))]
        public virtual TblUser User { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<Comments> Comments { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostMedia> PostMedia { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<PostTags> PostTags { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<Reactions> Reactions { get; set; }
        [InverseProperty("Post")]
        public virtual ICollection<UserMentions> UserMentions { get; set; }
    }
}
