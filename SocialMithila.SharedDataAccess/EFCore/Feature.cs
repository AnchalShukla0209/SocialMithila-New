using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("features")]
    public partial class Feature
    {
        [Key]
        [Column("feature_id")]
        public int FeatureId { get; set; }

        [Column("feature_name")]
        [Required, MaxLength(50)]
        public string FeatureName { get; set; }

        public ICollection<BusinessFeature> BusinessFeatures { get; set; }
    }
}
