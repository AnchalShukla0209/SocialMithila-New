using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel
{
    public class BusinessListViewModel
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string MainImageUrl { get; set; }
        public double Rating { get; set; }
        public double DistanceKm { get; set; }
        public string PaymentType { get; set; }
        public bool IsFollowing { get; set; }
    }

    public class PagedBusinessResult
    {
        public List<BusinessListViewModel> Businesses { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
