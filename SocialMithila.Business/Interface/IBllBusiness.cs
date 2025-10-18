using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.Business.Interface
{
    public interface IBllBusiness
    {
        Task<int> AddBusinessAsync(SocialMithila.SharedDataAccess.EFCore.Business business, List<string> imageUrls, List<string> amenities);
        Task<List<Category>> GetCategoriesAsync();
        Task<List<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId);
        Task<List<PaymentMethod>> GetPaymentMethodsAsync();

        PagedBusinessResult GetAllBusinesses(int userId, int page, int pageSize, string sortBy, string search);
        bool ToggleFollow(int userId, int businessId);

        BusinessDetailsViewModel GetBusinessById(int BusinessId);

    }
}
