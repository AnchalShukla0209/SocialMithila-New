using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class TblPostLike
    {
        [Key]
        public int Id { get; set; }
        public long PostId { get; set; }
        public int UserId { get; set; }
        public string ReactionType { get; set; }
        public long CommentId { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual TblUser User { get; set; }


    }
}
