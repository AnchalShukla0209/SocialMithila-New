using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class MessageReceipts
    {
        [Key]
        public long ReceiptId { get; set; }
        public long MessageId { get; set; }
        public int UserId { get; set; }
        public byte DeliveryStatus { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? StatusOn { get; set; }

        [ForeignKey(nameof(MessageId))]
        [InverseProperty(nameof(Messages.MessageReceipts))]
        public virtual Messages Message { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.MessageReceipts))]
        public virtual TblUser User { get; set; }
    }
}
