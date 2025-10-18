using Microsoft.EntityFrameworkCore;
using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.ResponseModel;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.Business.Business
{
    public class BllBusiness: IBllBusiness
    {
        private readonly AppDbContext _context;
        public BllBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddBusinessAsync(SocialMithila.SharedDataAccess.EFCore.Business business, List<string> imageUrls, List<string> amenities)
        {
            business.CreatedAt = DateTime.Now;
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();

            if (imageUrls != null && imageUrls.Any())
            {
                foreach (var img in imageUrls.Take(5))
                {
                    var image = new BusinessImage
                    {
                        BusinessId = business.BusinessId,
                        ImageUrl = img
                    };
                    _context.BusinessImages.Add(image);
                }
            }

            if (amenities != null && amenities.Any())
            {
                foreach (var text in amenities)
                {
                    var item = new BusinessAmenity
                    {
                        BusinessId = business.BusinessId,
                        AminitiesText = text,
                        IsActive = true,
                        IsDeleted = false,
                        AddedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    _context.TblBusinessAminities.Add(item);
                }
            }

            await _context.SaveChangesAsync();

            return business.BusinessId;
        }


        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId)
        {
            return await _context.SubCategories
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsAsync()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public PagedBusinessResult GetAllBusinesses(int userId, int page, int pageSize, string sortBy, string search)
        {
            var query = _context.Businesses.AsQueryable();

          
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.BusinessName.Contains(search) ||
                                         b.Category.CategoryName.Contains(search) ||
                                         b.SubCategory.SubCategoryName.Contains(search)||
                                         b.PaymentMethod.PaymentType.Contains(search));
            }

            switch (sortBy?.ToLower())
            {
                case "rating":
                    query = query.OrderByDescending(b => b.Rating);
                    break;
                case "distance":
                    query = query.OrderBy(b => b.DistanceKm);
                    break;
                default:
                    query = query.OrderBy(b => b.BusinessId); // Relevance
                    break;
            }

            int totalCount = query.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var businesses = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BusinessListViewModel
                {
                    BusinessId = b.BusinessId,
                    BusinessName = b.BusinessName,
                    CategoryName = b.Category.CategoryName,
                    SubCategoryName = b.SubCategory.SubCategoryName,
                    MainImageUrl = b.BusinessImages.FirstOrDefault() != null
                        ? b.BusinessImages.FirstOrDefault().ImageUrl
                        : "/content/images/no-image.png",
                    Rating = (double)(b.Rating ?? 0),
                    DistanceKm = (double)(b.DistanceKm ?? 0),
                    PaymentType = b.PaymentMethod != null ? b.PaymentMethod.PaymentType : string.Empty,
                    IsFollowing = _context.user_business_follows.Any(f => f.UserId == userId && f.BusinessId == b.BusinessId)
                })
                .ToList();

            return new PagedBusinessResult
            {
                Businesses = businesses,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }

        public bool ToggleFollow(int userId, int businessId)
        {
            var existing = _context.user_business_follows
                .FirstOrDefault(f => f.UserId == userId && f.BusinessId == businessId);

            if (existing != null)
            {
                _context.user_business_follows.Remove(existing);
                _context.SaveChanges();
                return false;
            }
            else
            {
                _context.user_business_follows.Add(new UserBusinessFollow
                {
                    UserId = userId,
                    BusinessId = businessId,
                    FollowDate = DateTime.Now
                });
                _context.SaveChanges();
                return true; 
            }
        }


        public BusinessDetailsViewModel GetBusinessById(int BusinessId)
        {
            var business = _context.Businesses
            .Include(b => b.Category)
            .Include(b => b.SubCategory)
            .Include(b => b.BusinessImages)
            .Include(b => b.BusinessReviews)
            .Include(b => b.BusinessFeatures)
            .FirstOrDefault(b => b.BusinessId == BusinessId);

            if (business == null)
                return null;

            

            var viewModel = new BusinessDetailsViewModel
            {
                BusinessId = business.BusinessId,
                BusinessName = business.BusinessName,
                CategoryName = business.Category?.CategoryName,
                SubCategoryName = business.SubCategory?.SubCategoryName,
                ContactNumber = business.ContactNumber,
                Address = business.Address,
                Description = business.Descreption,
                Rating = business.Rating,
                FeaturedImage = business.ImageUrl,
                CreatedAt = business.CreatedAt,

                Amenities = _context.TblBusinessAminities
                    .Where(a => a.BusinessId == BusinessId && !a.IsDeleted && a.IsActive)
                    .Select(a => a.AminitiesText)
                    .Distinct()
                    .ToList(),

                GalleryImages = _context.BusinessImages
                    .Where(img => img.BusinessId == BusinessId)
                    .Select(img => img.ImageUrl)
                    .ToList(),

                Reviews = _context.BusinessReviews
                    .Where(r => r.BusinessId == BusinessId)
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new BusinessReviewViewModel
                    {
                        ProfilePhoto = _context.TblUser.Where(u => u.Id == r.userid).Select(u => string.IsNullOrEmpty(u.ProfilePhoto)? "https://cdn-icons-png.flaticon.com/512/149/149071.png" : u.ProfilePhoto).FirstOrDefault(),
                        UserName = r.UserName,
                        Rating = r.Rating,
                        ReviewText = r.ReviewText,
                        CreatedAt = r.CreatedAt
                        }).ToList(),
                    TotalRatings = _context.BusinessReviews.Count(r => r.BusinessId == BusinessId),
                AverageRating = Math.Round(
    _context.BusinessReviews
        .Where(r => r.BusinessId == BusinessId)
        .Average(r => (decimal?)r.Rating) ?? 0,
    2
),

                TotalFollowers = _context.user_business_follows.Count(f => f.BusinessId == BusinessId),
                Followers = _context.user_business_follows
                .Where(f => f.BusinessId == BusinessId)
                .Select(f => new BusinessFollowerViewModel
                {
                    UserId = f.User.Id,
                    FullName = f.User.FirstName + " " + (f.User.LastName ?? ""),
                    ProfilePhoto = string.IsNullOrEmpty(f.User.ProfilePhoto)? "https://cdn-icons-png.flaticon.com/512/149/149071.png" : f.User.ProfilePhoto
                })
                .ToList()
            };

            return viewModel;
        }

    }
}
