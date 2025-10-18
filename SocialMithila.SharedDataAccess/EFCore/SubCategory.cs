using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("sub_categories")]
    public partial class SubCategory
    {
        [Key]
        [Column("sub_category_id")]
        public int SubCategoryId { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("sub_category_name")]
        [Required, MaxLength(100)]
        public string SubCategoryName { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public ICollection<Business> Businesses { get; set; }
    }
}
