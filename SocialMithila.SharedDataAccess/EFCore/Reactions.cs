using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Reactions
    {
        [Key]
        public long ReactionId { get; set; }
        public int UserId { get; set; }
        public long? PostId { get; set; }
        public long? CommentId { get; set; }
        [Required]
        [StringLength(30)]
        public string ReactionType { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey(nameof(CommentId))]
        [InverseProperty(nameof(Comments.Reactions))]
        public virtual Comments Comment { get; set; }
        [ForeignKey(nameof(PostId))]
        [InverseProperty(nameof(Posts.Reactions))]
        public virtual Posts Post { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.Reactions))]
        public virtual TblUser User { get; set; }
    }
}
