using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("TblStoryLike")]
    public class TblStoryLike
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Story))]
        public long StoryId { get; set; }

        [ForeignKey(nameof(LikedByUser))]
        public int LikedBy { get; set; }

        [ForeignKey(nameof(StoryCreatedByUser))]
        public int StoryCreatedBy { get; set; }

        [MaxLength(20)]
        public string ReactionType { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        // --- Navigation Properties ---
        public virtual Stories Story { get; set; }

        public virtual TblUser LikedByUser { get; set; }

        public virtual TblUser StoryCreatedByUser { get; set; }
    }
}
