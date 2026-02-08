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
    public interface IAdvertisementStatusService
    {
        Task<IEnumerable<AdvertisementStatusModel>> GetAllAsync();
        Task<AdvertisementStatusModel?> GetByIdAsync(int id);
        Task<AdvertisementStatusModel> CreateAsync(AdvertisementStatusModel advertisementStatus);
        Task<AdvertisementStatusModel?> UpdateAsync(int id, AdvertisementStatusModel advertisementStatus);
        Task<AdvertisementStatusModel?> DeleteAsync(int id, string username);
    }
    public class AdvertisementStatusService : IAdvertisementStatusService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;            // Dependency Injection of the AutoMapper
        public AdvertisementStatusService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Services Implementation of AdvertisementStatusService   
        public async Task<AdvertisementStatusModel> CreateAsync(AdvertisementStatusModel request)
        {
            try
            {
                var advertisementStatus = _mapper.Map<AdvertisementStatus>(request);
                // Name is Coming From Controller

                advertisementStatus.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                advertisementStatus.IsActive = true;
                advertisementStatus.Advertisements = new List<Advertisement>();

                // CreatedBy is extracted from Payload of JWT Token in Controller
                advertisementStatus.CreatedDate = DateTime.Now;

                await _dbContext.AdvertisementStatuses.AddAsync(advertisementStatus);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<AdvertisementStatusModel>(advertisementStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementStatusModel>> GetAllAsync()     // GetAll Active AdvertisementStatuses
        {
            try
            {
                return _mapper.Map<IEnumerable<AdvertisementStatusModel>>(await _dbContext.AdvertisementStatuses.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementStatusModel?> GetByIdAsync(int id)    // Get AdvertisementStatus By Id
        {
            try
            {
                return _mapper.Map<AdvertisementStatusModel>(await _dbContext.AdvertisementStatuses.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private  async Task<AdvertisementStatus?> GetById(int id)    // Get AdvertisementStatus By Id
        {
            try
            {
                return await _dbContext.AdvertisementStatuses.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementStatusModel?> UpdateAsync(int id, AdvertisementStatusModel advertisementStatus)   // Update AdvertisementStatus By Id
        {
            try
            {
                var found = await GetById(id);
                if (found != null)
                {
                    found.Name = advertisementStatus.Name;

                    // LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = advertisementStatus.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    //_dbContext.AdvertisementStatuses.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementStatusModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementStatusModel?> DeleteAsync(int id, string username)   // Soft Delete AdvertisementStatus By Id
        {
            try
            {
                var found = await GetById(id);
                if (found != null)
                {
                    found.IsActive = false;

                    // LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    //_dbContext.AdvertisementStatuses.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementStatusModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }




        }
    }
}