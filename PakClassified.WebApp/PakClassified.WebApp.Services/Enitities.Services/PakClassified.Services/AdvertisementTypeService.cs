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
    public interface IAdvertisementTypeService
    {
        Task<IEnumerable<AdvertisementTypeModel>> GetAllAsync();
        Task<AdvertisementTypeModel?> GetByIdAsync(int id);
        Task<AdvertisementTypeModel> CreateAsync(AdvertisementTypeModel advertisementSubCategory);
        Task<AdvertisementTypeModel?> UpdateAsync(int id, AdvertisementTypeModel advertisementSubCategory);
        Task<AdvertisementTypeModel?> DeleteAsync(int id, string username);
    }
    public class AdvertisementTypeService : IAdvertisementTypeService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;      // Dependency Injection of the Mapper
        public AdvertisementTypeService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Services Implementation of AdvertisementTypeService
        public async Task<AdvertisementTypeModel> CreateAsync(AdvertisementTypeModel request)          // Create New AdvertisementType
        {
            try
            {
                var advertisementType = _mapper.Map<AdvertisementType>(request);
                // Name is Coming From Controller

                advertisementType.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                advertisementType.IsActive = true;
                advertisementType.Advertisements = new List<Advertisement>();

                // CreatedBy is extracted from Payload of JWT Token in Controller
                advertisementType.CreatedDate = DateTime.Now;

                await _dbContext.AdvertisementTypes.AddAsync(advertisementType);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AdvertisementTypeModel>(advertisementType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementTypeModel>> GetAllAsync()     // GetAll Active AdvertisementTypes
        {
            try
            {
                return _mapper.Map<IEnumerable<AdvertisementTypeModel>>(await _dbContext.AdvertisementTypes.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementTypeModel?> GetByIdAsync(int id)    // Get AdvertisementType By Id
        {
            try
            {
                return _mapper.Map<AdvertisementTypeModel>(await _dbContext.AdvertisementTypes.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementTypeModel?> UpdateAsync(int id, AdvertisementTypeModel advertisementType)    // Update AdvertisementType By Id
        {
            try
            {
                var found = _mapper.Map<AdvertisementType>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = advertisementType.Name;

                    // LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = advertisementType.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.AdvertisementTypes.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementTypeModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementTypeModel?> DeleteAsync(int id, string username)    // Soft Delete AdvertisementType By Id
        {
            try
            {
                var found = _mapper.Map<AdvertisementType>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    // LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.AdvertisementTypes.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementTypeModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
