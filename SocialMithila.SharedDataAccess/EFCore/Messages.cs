using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Messages
    {
        public Messages()
        {
            MessageAttachments = new HashSet<MessageAttachments>();
            MessageReceipts = new HashSet<MessageReceipts>();
        }

        [Key]
        public long MessageId { get; set; }
        public long ConversationId { get; set; }
        public int SenderId { get; set; }
        public string ContentText { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? EditedOn { get; set; }

        [ForeignKey(nameof(ConversationId))]
        [InverseProperty(nameof(Conversations.Messages))]
        public virtual Conversations Conversation { get; set; }
        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(TblUser.Messages))]
        public virtual TblUser Sender { get; set; }
        [InverseProperty("Message")]
        public virtual ICollection<MessageAttachments> MessageAttachments { get; set; }
        [InverseProperty("Message")]
        public virtual ICollection<MessageReceipts> MessageReceipts { get; set; }
    }
}
