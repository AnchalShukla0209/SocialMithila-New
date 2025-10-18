using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Notifications
    {
        [Key]
        public long NotificationId { get; set; }
        public int UserId { get; set; }
        public int? ActorId { get; set; }
        public int? PostId { get; set; }
        [Required]
        [StringLength(50)]
        public string NotificationType { get; set; }
        [StringLength(200)]
        public string Message { get; set; }
        [StringLength(50)]
        public string EntityType { get; set; }
        public long? EntityId { get; set; }
        public string RequestStatus { get; set; }
        public bool? IsRead { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey(nameof(ActorId))]
        [InverseProperty(nameof(TblUser.NotificationsActor))]
        public virtual TblUser Actor { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.NotificationsUser))]
        public virtual TblUser User { get; set; }
    }
}
