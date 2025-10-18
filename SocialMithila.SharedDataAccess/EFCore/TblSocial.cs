using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class TblSocial
    {
        [Key]
        public int SocialId { get; set; }
        public int UserId { get; set; }
        [StringLength(200)]
        public string FacebookProfile { get; set; }
        [StringLength(200)]
        public string Twitter { get; set; }
        [StringLength(200)]
        public string LinkedIn { get; set; }
        [StringLength(200)]
        public string Instagram { get; set; }
        [StringLength(200)]
        public string Flickr { get; set; }
        [StringLength(200)]
        public string Github { get; set; }
        [StringLength(200)]
        public string Skype { get; set; }
        [StringLength(200)]
        public string Google { get; set; }
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
        [InverseProperty(nameof(TblUser.TblSocial))]
        public virtual TblUser User { get; set; }
    }
}
