using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("Contact")]
    public partial class Contact
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Column("Message")]
        [MaxLength(1000)]
        public string Message { get; set; }

        [Column("MobileNumber")]
        [MaxLength(20)]
        public string MobileNumber { get; set; }

        [Column("isDeleted")]
        public bool IsDeleted { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        // Optional: if you have a related User table
        // [ForeignKey(nameof(UserId))]
        // public virtual User? User { get; set; }
    }
}
