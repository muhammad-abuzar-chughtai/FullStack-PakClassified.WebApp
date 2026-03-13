using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.Locations;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<AdvertisementModel>> GetAllAsync();
        Task<AdvertisementModel?> GetByIdAsync(int id);
        Task<AdvertisementModel> CreateAsync(AdvertisementModel advertisement);
        Task<AdvertisementModel?> UpdateAsync(int id, AdvertisementModel advertisement);
        Task<AdvertisementModel?> DeleteAsync(int id, string username);
        Task<IEnumerable<AdvertisementModel>> SearchAsync(AdvertisementSearchFilterModel filter);
    }
    public class AdvertisementService : IAdvertisementService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;              // Dependency Injection of the AutoMapper
        private readonly IAdvertisementImageService _advertisementImageService; // Dependency Injection of AdvertisementImageService
        public AdvertisementService(AppDBContext dbContext, IMapper mapper, IAdvertisementImageService advertisementImageService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _advertisementImageService = advertisementImageService;
        }

        // Services Implementation of AdvertisementService
        public async Task<AdvertisementModel> CreateAsync(AdvertisementModel request)          // Create New Advertisement
        {
            try
            {
                var advertisement = _mapper.Map<Advertisement>(request);
                // Name is Coming From Controller
                // Title is Coming From Controller
                // Description is Coming From Controller
                // Decimal is Coming From Controller
                // Likes is Coming From Controller
                // StartsOn is Coming From Controller
                // EndsOn is Coming From Controller

                advertisement.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                advertisement.IsActive = true;
                advertisement.Tags = new List<AdvertisementTag>();
                advertisement.Images = new List<AdvertisementImage>();

                // CityAreaId is Coming From Controller
                // PostedById is Coming From Controller
                // StatusId is Coming From Controller
                // TypeId is Coming From Controller
                // SubCategoryId is Coming From Controller


                // CreatedBy is extracted from Payload of JWT Token in Controller
                advertisement.CreatedDate = DateTime.Now;

                await _dbContext.Advertisements.AddAsync(advertisement);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AdvertisementModel>(advertisement);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementModel>> GetAllAsync()     // GetAll Active Advertisements
        {
            try
            {
                return await _dbContext.Advertisements.Where(a => a.IsActive)
                .Select(a => new AdvertisementModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    Likes = a.Likes,
                    StartsOn = a.StartsOn,
                    EndsOn = a.EndsOn,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    LastModifiedBy = a.LastModifiedBy,
                    CityAreaId = a.CityAreaId,
                    PostedById = a.PostedById,
                    StatusId = a.StatusId,
                    TypeId = a.TypeId,
                    SubCategoryId = a.SubCategoryId,
                    TagsId = a.Tags.Where(t => t.IsActive).Select(t => t.Id).ToList(),
                    Images = a.Images.Where(i => i.IsActive).Select(i => i.Id).ToList()
                })
                .OrderByDescending(a => a.CreatedDate).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementModel?> GetByIdAsync(int id)      // Get Advertisement By Id
        {
            try
            {
                return await _dbContext.Advertisements.Where(a => a.IsActive && a.Id == id)
                .Select(a => new AdvertisementModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    Likes = a.Likes,
                    StartsOn = a.StartsOn,
                    EndsOn = a.EndsOn,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    LastModifiedBy = a.LastModifiedBy,
                    CityAreaId = a.CityAreaId,
                    PostedById = a.PostedById,
                    StatusId = a.StatusId,
                    TypeId = a.TypeId,
                    SubCategoryId = a.SubCategoryId,
                    TagsId = a.Tags.Where(t => t.IsActive).Select(t => t.Id).ToList(),
                    Images = a.Images.Where(i => i.IsActive).Select(i => i.Id).ToList()
                })
                .OrderByDescending(a => a.CreatedDate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        private async Task<Advertisement?> GetById(int id)      // Get Advertisement By Id
        {
            try
            {
                return await _dbContext.Advertisements.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementModel?> UpdateAsync(int id, AdvertisementModel dto)
        {
            var ad = await _dbContext.Advertisements
                .Include(a => a.Images)
                .Include(a => a.Tags)
                .FirstOrDefaultAsync(a => a.IsActive && a.Id == id);
            if (ad == null) return null;

            ad.Name = dto.Name;
            ad.Title = dto.Title;
            ad.Description = dto.Description;
            ad.Price = dto.Price;
            ad.Likes = dto.Likes;
            ad.StartsOn = dto.StartsOn;
            ad.EndsOn = dto.EndsOn;

            ad.CityAreaId = dto.CityAreaId;
            ad.PostedById = dto.PostedById;
            ad.StatusId = dto.StatusId;
            ad.TypeId = dto.TypeId;
            ad.SubCategoryId = dto.SubCategoryId;

            //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
            ad.LastModifiedBy = dto.LastModifiedBy;
            ad.LastModifiedDate = DateTime.Now;

            // ✅ Handle Images (One-to-Many)

            // REMOVE
            var imagesToRemove = ad.Images
                .Where(i => !dto.Images.Contains(i.Id))
                .ToList();
            foreach (var img in imagesToRemove)
                ad.Images.Remove(img);

            // ADD
            var currentImageIds = ad.Images.Select(i => i.Id).ToHashSet();
            var imageIdsToAdd = dto.Images.Except(currentImageIds).ToList();
            if (imageIdsToAdd.Any())
            {
                var imagesToAdd = await _dbContext.AdvertisementImages
                    .Where(i => imageIdsToAdd.Contains(i.Id) && i.IsActive)
                    .ToListAsync();
                foreach (var img in imagesToAdd)
                    ad.Images.Add(img);
            }

            // ✅ Handle Tags (Many-to-Many)
            var currentTagIds = ad.Tags.Select(t => t.Id).ToHashSet();
            var desiredTagIds = dto.TagsId.ToHashSet();

            // ADD
            var tagIdsToAdd = desiredTagIds.Except(currentTagIds).ToList();
            if (tagIdsToAdd.Any())
            {
                var tagsToAdd = await _dbContext.AdvertisementTags
                    .Where(t => tagIdsToAdd.Contains(t.Id) && t.IsActive)
                    .ToListAsync();
                foreach (var tag in tagsToAdd)
                    ad.Tags.Add(tag);
            }

            // REMOVE
            var tagsToRemove = ad.Tags
                .Where(t => currentTagIds.Except(desiredTagIds).Contains(t.Id))
                .ToList();
            foreach (var tag in tagsToRemove)
                ad.Tags.Remove(tag); // EF handles AdvertisementTagMapping delete automatically

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<AdvertisementModel>(ad);

        }

        public async Task<AdvertisementModel?> DeleteAsync(int id, string username)      // Soft Delete Advertisement By Id
        {
            try
            {
                var found = await GetById(id);
                if (found != null)
                {
                    found.IsActive = false;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Advertisements.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementModel>> SearchAsync(AdvertisementSearchFilterModel filter)    //Used to Filter Advertisements based on SubCategory, Type, Status, Tags, PostedBy and CityArea.
        {
            var query = _dbContext.Advertisements.AsNoTracking().AsQueryable();

            // SubCategory filter
            if (filter.SubCategoryId.HasValue)
            {
                query = query.Where(a => a.SubCategoryId == filter.SubCategoryId.Value);
            }

            // Type filter
            if (filter.TypeId.HasValue)
            {
                query = query.Where(a => a.TypeId == filter.TypeId.Value);
            }

            // Status filter
            if (filter.StatusId.HasValue)
            {
                query = query.Where(a => a.StatusId == filter.StatusId.Value);
            }

            // Tag filter
            if (filter.TagIds?.Any() == true)
            {
                query = query.Where(a => a.Tags.Any(at => filter.TagIds.Contains(at.Id)));
            }

            // PostedBy filter
            if (filter.PostedById.HasValue)
            {
                query = query.Where(a => a.PostedById == filter.PostedById.Value);
            }

            // CityArea filter
            if (filter.CityAreaId.HasValue)
            {
                query = query.Where(a => a.CityAreaId == filter.CityAreaId.Value);
            }

            return _mapper.Map<IEnumerable<AdvertisementModel>>(await query.OrderByDescending(a => a.CreatedDate).ToListAsync());
        }

        
    }
}
