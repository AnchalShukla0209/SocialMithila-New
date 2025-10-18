using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class TblAddress
    {
        [Key]
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        [Required]
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(10)]
        public string PinCode { get; set; }
        public string GoogleMapLocation { get; set; }
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
        [InverseProperty(nameof(TblUser.TblAddress))]
        public virtual TblUser User { get; set; }
    }
}
