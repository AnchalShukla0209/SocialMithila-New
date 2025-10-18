using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Comments
    {
        public Comments()
        {
            Reactions = new HashSet<Reactions>();
            UserMentions = new HashSet<UserMentions>();
        }

        [Key]
        public long CommentId { get; set; }
        public long PostId { get; set; }
        public int UserId { get; set; }
        public long? ParentCommentId { get; set; }
        [Required]
        [StringLength(1000)]
        public string CommentText { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty(nameof(Posts.Comments))]
        public virtual Posts Post { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.Comments))]
        public virtual TblUser User { get; set; }
        [InverseProperty("Comment")]
        public virtual ICollection<Reactions> Reactions { get; set; }
        [InverseProperty("Comment")]
        public virtual ICollection<UserMentions> UserMentions { get; set; }
    }
}
