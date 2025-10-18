using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("payment_methods")]
    public partial class PaymentMethod
    {
        [Key]
        [Column("payment_id")]
        public int PaymentId { get; set; }

        [Column("payment_type")]
        [Required, MaxLength(50)]
        public string PaymentType { get; set; }

        public ICollection<Business> Businesses { get; set; }
    }
}
