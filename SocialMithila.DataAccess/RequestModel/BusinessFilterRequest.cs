using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.RequestModel
{
    public class BusinessFilterRequest
    {
        public int? CategoryId { get; set; }
        public List<int> SubCategoryIds { get; set; }
        public List<int> ShopIds { get; set; }
        public List<int> Ratings { get; set; }
        public List<int> FollowerIds { get; set; }
        public int Page { get; set; } = 1;
    }

}
