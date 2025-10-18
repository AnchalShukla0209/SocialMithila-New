using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Conversations
    {
        public Conversations()
        {
            ConversationMembers = new HashSet<ConversationMembers>();
            Messages = new HashSet<Messages>();
        }

        [Key]
        public long ConversationId { get; set; }
        public bool IsGroup { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? LastMessageOn { get; set; }

        [InverseProperty("Conversation")]
        public virtual ICollection<ConversationMembers> ConversationMembers { get; set; }
        [InverseProperty("Conversation")]
        public virtual ICollection<Messages> Messages { get; set; }
    }
}
