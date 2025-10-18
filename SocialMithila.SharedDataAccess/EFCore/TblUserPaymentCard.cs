using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class TblUserPaymentCard
    {
        [Key]
        public int PaymentCardId { get; set; }
        [Required]
        [StringLength(20)]
        public string CardNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string HolderName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int UserId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.TblUserPaymentCard))]
        public virtual TblUser User { get; set; }
    }
}
