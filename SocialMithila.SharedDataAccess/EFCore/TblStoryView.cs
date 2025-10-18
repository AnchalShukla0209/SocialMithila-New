using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMithila.SharedDataAccess.EFCore
{

    [Table("TblStoryView")]
    public class TblStoryView
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Story))]
        public long StoryId { get; set; }

        [ForeignKey(nameof(ViewedByUser))]
        public int ViewedBy { get; set; }

        public DateTime ViewedOn { get; set; } = DateTime.Now;

        public bool IsLiked { get; set; } = false; // auto-updated when user likes

        // Navigation
        public virtual Stories Story { get; set; }
        public virtual TblUser ViewedByUser { get; set; }
    }

}
