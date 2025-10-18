using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel
{
    public class BusinessDetailsViewModel
    {
        // Basic Business Info
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public decimal? Rating { get; set; }
        public string FeaturedImage { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<string> Amenities { get; set; } = new List<string>();
        public List<string> GalleryImages { get; set; } = new List<string>();
        public List<BusinessReviewViewModel> Reviews { get; set; } = new List<BusinessReviewViewModel>();
        public int TotalRatings { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalFollowers { get; set; }
        public List<BusinessFollowerViewModel> Followers { get; set; } = new List<BusinessFollowerViewModel>();
        public List<RatingCategory> Categories { get; set; }
    }

    public class BusinessReviewViewModel
    {
        public string UserName { get; set; }
        public string ProfilePhoto { get; set; }
        public decimal? Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class BusinessFollowerViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string ProfilePhoto { get; set; }
    }

    public class RatingCategory
    {
        public string Name { get; set; }
        public decimal Rating { get; set; } 
    }

}
