using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class ConversationMembers
    {
        [Key]
        public long ConversationMemberId { get; set; }
        public long ConversationId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? JoinedOn { get; set; }
        public bool? IsMuted { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ConversationId))]
        [InverseProperty(nameof(Conversations.ConversationMembers))]
        public virtual Conversations Conversation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.ConversationMembers))]
        public virtual TblUser User { get; set; }
    }
}
