using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class Groups
    {
        public Groups()
        {
            GroupMembers = new HashSet<GroupMembers>();
        }

        [Key]
        public long GroupId { get; set; }
        [Required]
        [StringLength(200)]
        public string GroupName { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(TblUser.Groups))]
        public virtual TblUser CreatedByNavigation { get; set; }
        [InverseProperty("Group")]
        public virtual ICollection<GroupMembers> GroupMembers { get; set; }
    }
}
