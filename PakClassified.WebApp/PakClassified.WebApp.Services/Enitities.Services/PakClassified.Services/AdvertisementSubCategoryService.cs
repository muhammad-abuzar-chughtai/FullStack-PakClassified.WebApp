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

namespace b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services
{
    public interface IAdvertisementSubCategoryService
    {
        Task<IEnumerable<AdvertisementSubCategoryModel>> GetAllAsync();
        Task<AdvertisementSubCategoryModel?> GetByIdAsync(int id);
        Task<AdvertisementSubCategoryModel> CreateAsync(AdvertisementSubCategoryModel advertisementSubCategory);
        Task<AdvertisementSubCategoryModel?> UpdateAsync(int id, AdvertisementSubCategoryModel advertisementSubCategory);
        Task<AdvertisementSubCategoryModel?> DeleteAsync(int id, string username);
    }
    public class AdvertisementSubCategoryService : IAdvertisementSubCategoryService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;
        public AdvertisementSubCategoryService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        // Services Implementation of AdvertisementSubCategoryService   
        public async Task<AdvertisementSubCategoryModel> CreateAsync(AdvertisementSubCategoryModel request)
        {
            try
            {
                var SubCategory = _mapper.Map<AdvertisementSubCategory>(request);
                // Name is Coming From Controller
                // Description is Coming From Controller
                // CategoryId is Coming From Controller


                SubCategory.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                SubCategory.IsActive = true;
                SubCategory.Advertisements = new List<Advertisement>();

                // CreatedBy is extracted from Payload of JWT Token in Controller
                SubCategory.CreatedDate = DateTime.Now;

                await _dbContext.AdvertisementSubCategories.AddAsync(SubCategory);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AdvertisementSubCategoryModel>(SubCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementSubCategoryModel>> GetAllAsync()     // GetAll Active AdvertisementSubCategories
        {
            try
            {
                return _mapper.Map<IEnumerable<AdvertisementSubCategoryModel>>(await _dbContext.AdvertisementSubCategories.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementSubCategoryModel?> GetByIdAsync(int id)      // Get AdvertisementSubCategory By Id
        {
            try
            {
                return _mapper.Map<AdvertisementSubCategoryModel>(await _dbContext.AdvertisementSubCategories.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementSubCategoryModel?> UpdateAsync(int id, AdvertisementSubCategoryModel advertisementSubCategory)
        {
            try
            {
                var found = _mapper.Map<AdvertisementSubCategory>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = advertisementSubCategory.Name;
                    found.Description = advertisementSubCategory.Description;
                    found.CategoryId = advertisementSubCategory.CategoryId;
                    
                    found.LastModifiedBy = advertisementSubCategory.LastModifiedBy; // Extracted from JWT Token in Controller
                    found.LastModifiedDate = DateTime.Now;
                    
                    _dbContext.AdvertisementSubCategories.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementSubCategoryModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementSubCategoryModel?> DeleteAsync(int id, string username)
        {
            try
            {
                var found = _mapper.Map<AdvertisementSubCategory>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;
                    
                    _dbContext.AdvertisementSubCategories.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementSubCategoryModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }




        }
    }
