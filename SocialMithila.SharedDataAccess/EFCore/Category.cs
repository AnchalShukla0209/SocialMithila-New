using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("categories")]
    public partial class Category
    {
        [Key]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("category_name")]
        [Required, MaxLength(100)]
        public string CategoryName { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<Business> Businesses { get; set; }
    }
}
