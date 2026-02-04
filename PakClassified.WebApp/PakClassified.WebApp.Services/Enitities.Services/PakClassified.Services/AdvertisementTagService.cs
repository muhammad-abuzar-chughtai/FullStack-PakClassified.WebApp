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
    public interface IAdvertisementTagService
    {
        Task<IEnumerable<AdvertisementTagModel>> GetAllAsync();
        Task<AdvertisementTagModel?> GetByIdAsync(int id);
        Task<AdvertisementTagModel> CreateAsync(AdvertisementTagModel advertisementTag);
        Task<AdvertisementTagModel?> UpdateAsync(int id, AdvertisementTagModel advertisementTag);
        Task<AdvertisementTagModel?> DeleteAsync(int id, string username);
    }
    public class AdvertisementTagService : IAdvertisementTagService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;            // Dependency Injection of the AutoMapper
        public AdvertisementTagService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Services Implementation of AdvertisementTagService
        public async Task<AdvertisementTagModel> CreateAsync(AdvertisementTagModel request)
        {
            try
            {
                var advertisementTag = _mapper.Map<AdvertisementTag>(request);
                // Name is Coming From Controller
                // NoOfSearch is Coming From Controller

                advertisementTag.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                advertisementTag.IsActive = true;
                advertisementTag.Advertisements = new List<Advertisement>();

                // CreatedBy is extracted from Payload of JWT Token in Controller
                advertisementTag.CreatedDate = DateTime.Now;

                await _dbContext.AdvertisementTags.AddAsync(advertisementTag);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AdvertisementTagModel>(advertisementTag);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementTagModel>> GetAllAsync()     // GetAll Active AdvertisementTags
        {
            try
            {
                return _mapper.Map<IEnumerable<AdvertisementTagModel>>(await _dbContext.AdvertisementTags.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementTagModel?> GetByIdAsync(int id)    // Get AdvertisementTag By Id
        {
            try
            {
                return _mapper.Map<AdvertisementTagModel>(await _dbContext.AdvertisementTags.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementTagModel?> UpdateAsync(int id, AdvertisementTagModel advertisementTag)
        {
            try
            {
                var found = _mapper.Map<AdvertisementTag>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = advertisementTag.Name;
                    found.NoOfSearch = advertisementTag.NoOfSearch;

                    // LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = advertisementTag.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.AdvertisementTags.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementTagModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementTagModel?> DeleteAsync(int id, string username)
        {
            try
            {
                var found = _mapper.Map<AdvertisementTag>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    // LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.AdvertisementTags.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementTagModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
