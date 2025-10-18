using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("Tble_UserPassword")]
    public partial class TbleUserPassword
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(255)]
        public string NewPassword { get; set; }
        public bool IsBlocked { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TblUser.TbleUserPassword))]
        public virtual TblUser User { get; set; }
    }
}
