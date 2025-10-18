using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class MessageAttachments
    {
        [Key]
        public long AttachmentId { get; set; }
        public long MessageId { get; set; }
        [Required]
        [StringLength(500)]
        public string MediaUrl { get; set; }
        [Required]
        [StringLength(30)]
        public string MediaType { get; set; }
        public long? SizeBytes { get; set; }

        [ForeignKey(nameof(MessageId))]
        [InverseProperty(nameof(Messages.MessageAttachments))]
        public virtual Messages Message { get; set; }
    }
}
