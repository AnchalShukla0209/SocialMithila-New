using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class ContentReports
    {
        [Key]
        public long ReportId { get; set; }
        public int ReporterId { get; set; }
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }
        public long EntityId { get; set; }
        [StringLength(500)]
        public string Reason { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        public bool? IsResolved { get; set; }

        [ForeignKey(nameof(ReporterId))]
        [InverseProperty(nameof(TblUser.ContentReports))]
        public virtual TblUser Reporter { get; set; }
    }
}
