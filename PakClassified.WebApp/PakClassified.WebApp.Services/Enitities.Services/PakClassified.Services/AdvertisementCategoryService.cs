using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.Locations;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services
{
    public interface IAdvertisementCategoryService
    {
        Task<IEnumerable<AdvertisementCategoryModel>> GetAllAsync();
        Task<AdvertisementCategoryModel?> GetByIdAsync(int id);
        Task<AdvertisementCategoryModel> CreateAsync(AdvertisementCategoryModel advertisementCategory);
        Task<AdvertisementCategoryModel?> UpdateAsync(int id, AdvertisementCategoryModel advertisementCategory);
        Task<AdvertisementCategoryModel?> DeleteAsync(int id, string username);
    }
    public class AdvertisementCategoryService : IAdvertisementCategoryService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;
        public AdvertisementCategoryService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Services Implementation of AdvertisementCategoryService
        public async Task<AdvertisementCategoryModel> CreateAsync(AdvertisementCategoryModel request)          // Create New AdvertisementCategory
        {
            try
            {
                var advertisementCategory = _mapper.Map<AdvertisementCategory>(request);
                // Name is Coming From Controller
                // Description is Coming From Controller

                advertisementCategory.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                advertisementCategory.IsActive = true;
                advertisementCategory.SubCategories = new List<AdvertisementSubCategory>();

                // CreatedBy is extracted from Payload of JWT Token in Controller
                advertisementCategory.CreatedDate = DateTime.Now;

                await _dbContext.AdvertisementCategories.AddAsync(advertisementCategory);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AdvertisementCategoryModel>(advertisementCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementCategoryModel>> GetAllAsync()     // GetAll Active AdvertisementCategories
        {
            try
            {
                return _mapper.Map<IEnumerable<AdvertisementCategoryModel>>(await _dbContext.AdvertisementCategories.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementCategoryModel?> GetByIdAsync(int id)      // Get AdvertisementCategory By Id
        {
            try
            {
                return _mapper.Map<AdvertisementCategoryModel>(await _dbContext.AdvertisementCategories.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<AdvertisementCategory?> GetById(int id)      // Get AdvertisementCategory By Id
        {
            try
            {
                return await _dbContext.AdvertisementCategories.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementCategoryModel?> UpdateAsync(int id, AdvertisementCategoryModel advertisementCategory)      // Update AdvertisementCategory By Id
        {
            try
            {
                var found = await GetById(id);
                if (found != null)
                {
                    found.Name = advertisementCategory.Name;
                    found.Description = advertisementCategory.Description;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = advertisementCategory.LastModifiedBy; 
                    found.LastModifiedDate = DateTime.Now;
                    
                    //_dbContext.AdvertisementCategories.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementCategoryModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementCategoryModel?> DeleteAsync(int id, string username)      // Soft Delete AdvertisementCategory By Id
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
                    
                    //_dbContext.AdvertisementCategories.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementCategoryModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}