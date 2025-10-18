using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    [Table("business_features")]
    public partial class BusinessFeature
    {
        [Column("business_id")]
        public int BusinessId { get; set; }

        [Column("feature_id")]
        public int FeatureId { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public Business Business { get; set; }

        [ForeignKey(nameof(FeatureId))]
        public Feature Feature { get; set; }
    }
}
