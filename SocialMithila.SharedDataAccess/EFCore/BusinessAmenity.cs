using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("TblBusinessAminities")]
    public partial class BusinessAmenity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Business")]
        public int BusinessId { get; set; }

        [Required]
        [MaxLength(500)]
        public string AminitiesText { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Navigation property
        public virtual Business Business { get; set; }
    }
}
