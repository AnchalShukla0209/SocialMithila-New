using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class UserMentions
    {
        [Key]
        public int MentionId { get; set; }
        public long? PostId { get; set; }
        public long? CommentId { get; set; }
        public int? MentionedUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [ForeignKey(nameof(CommentId))]
        [InverseProperty(nameof(Comments.UserMentions))]
        public virtual Comments Comment { get; set; }
        [ForeignKey(nameof(MentionedUserId))]
        [InverseProperty(nameof(TblUser.UserMentions))]
        public virtual TblUser MentionedUser { get; set; }
        [ForeignKey(nameof(PostId))]
        [InverseProperty(nameof(Posts.UserMentions))]
        public virtual Posts Post { get; set; }
    }
}
