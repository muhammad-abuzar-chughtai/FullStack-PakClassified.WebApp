using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
                return _mapper.Map<IEnumerable<AdvertisementModel>>(await _dbContext.Advertisements.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
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
                return _mapper.Map<AdvertisementModel>(await _dbContext.Advertisements.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementModel?> UpdateAsync(int id, AdvertisementModel advertisement)      // Update Advertisement By Id
        {
            try
            {

                var found = await _dbContext.Advertisements.Where(c => c.IsActive && c.Id == id).Include(a => a.Tags).Include(a => a.Images).FirstOrDefaultAsync();
                if (found != null)
                {

                    found.Name = advertisement.Name;
                    found.Title = advertisement.Title;
                    found.Description = advertisement.Description;
                    found.Price = advertisement.Price;
                    found.Likes = advertisement.Likes;
                    found.StartsOn = advertisement.StartsOn;
                    found.EndsOn = advertisement.EndsOn;

                    found.CityAreaId = advertisement.CityAreaId;
                    found.PostedById = advertisement.PostedById;
                    found.StatusId = advertisement.StatusId;
                    found.TypeId = advertisement.TypeId;
                    found.SubCategoryId = advertisement.SubCategoryId;


                    UpdateTag(found, advertisement);
                    UpdateImages(found, advertisement);

                    
                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = advertisement.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    //_dbContext.Advertisements.Update(found);
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

        private async Task UpdateTag(Advertisement found, AdvertisementModel advertisement)
        {
            var existingTagIds = found.Tags.Select(t => t.Id).ToHashSet();

            var incomingTagIds = advertisement.Tags != null ? advertisement.Tags.Select(t => t.Id).ToHashSet() : new HashSet<int>();

            var tagsToRemove = found.Tags.Where(t => !incomingTagIds.Contains(t.Id)).ToList();

            foreach (var tag in tagsToRemove)
            {
                found.Tags.Remove(tag);
            }

            var tagIdsToAdd = incomingTagIds.Where(id => !existingTagIds.Contains(id)).ToList();

            var tagsToAdd = await _dbContext.AdvertisementTags.Where(t => tagIdsToAdd.Contains(t.Id)).ToListAsync();

            foreach (var tag in tagsToAdd)
            {
                found.Tags.Add(tag);
            }
        }

        //private async Task UpdateImages(Advertisement found, AdvertisementModel advertisement)
        //{
        //    var existingImageIds = found.Images.Select(i => i.Id).ToHashSet();

        //    var incomingExistingIds = advertisement.Images.Where(i => i.Id > 0).Select(i => i.Id).ToHashSet();

        //    var imagesToRemove = existingImageIds.Except(incomingExistingIds).ToList();

        //    // NEW images
        //    var newImages = advertisement.Images.Where(i => i.Id == 0 && i.Content != null).ToList();

        //    if (imagesToRemove.Any())
        //    {
        //        await _advertisementImageService.SyncAsync(
        //            found.Id,
        //            removeIds: imagesToRemove
        //        );
        //    }

        //    foreach (var img in newImages)
        //    {
        //        await _advertisementImageService.CreateAsync(img);
        //    }


        //}
        public async Task<AdvertisementModel?> DeleteAsync(int id, string username)      // Soft Delete Advertisement By Id
        {
            try
            {
                var found = _mapper.Map<Advertisement>(await GetByIdAsync(id));
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
            if (filter.PostedById.HasValue)
            {
                query = query.Where(a => a.CityAreaId == filter.CityAreaId.Value);
            }

            return _mapper.Map<IEnumerable<AdvertisementModel>>(await query.OrderByDescending(a => a.CreatedDate).ToListAsync());
        }
    }
}
