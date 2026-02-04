using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.Locations;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PakClassified.WebApp.DTOs.Location.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b._PakClassified.WebApp.Services.Enitities.Services.Location.Services
{
    public interface ICityAreaService
    {
        Task<IEnumerable<CityAreaModel>> GetAllAsync();
        Task<CityAreaModel?> GetByIdAsync(int id);
        Task<CityAreaModel> CreateAsync(CityAreaModel cityArea);
        Task<CityAreaModel?> UpdateAsync(int id, CityAreaModel cityArea);
        Task<CityAreaModel?> DeleteAsync(int id, string username);
    }
    public class CityAreaService : ICityAreaService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;    // Dependency Injection of the Mapper
        public CityAreaService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Services Implementation of CityAreaService
        public async Task<CityAreaModel> CreateAsync(CityAreaModel request)          // Create New CityArea
        {
            try
            {
                var cityArea = _mapper.Map<CityArea>(request);

                // cityArea.Name is Coming From Controller
                // cityArea.CityId is Coming From Controller
                cityArea.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                cityArea.IsActive = true;
                cityArea.Advertisements = new List<Advertisement>();

                // cityArea.CreatedBy is extracted from Payload of JWT Token in Controller
                cityArea.CreatedDate = DateTime.Now;

                await _dbContext.CityAreas.AddAsync(cityArea);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<CityAreaModel>(cityArea);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CityAreaModel>> GetAllAsync()     // GetAll Active CityAreas
        {
            try
            {
                return _mapper.Map<IEnumerable<CityAreaModel>>(await _dbContext.CityAreas.Where(ca => ca.IsActive).OrderByDescending(ca => ca.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CityAreaModel?> GetByIdAsync(int id)      // Get CityArea By Id
        {
            try
            {
                return _mapper.Map<CityAreaModel>(await _dbContext.CityAreas.Where(ca => ca.IsActive && ca.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CityAreaModel?> UpdateAsync(int id, CityAreaModel cityArea)      // Update CityArea By Id
        {
            try
            {
                var found = _mapper.Map<CityArea>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = cityArea.Name;
                    found.CityId = cityArea.CityId;

                    //city.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = cityArea.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.CityAreas.Update(found);
                    await _dbContext.SaveChangesAsync();
                }

                return _mapper.Map<CityAreaModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CityAreaModel?> DeleteAsync(int id, string username)    // Soft Delete CityArea By Id
        {
            try
            {
                var found = _mapper.Map<CityArea>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    //cityArea.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.CityAreas.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<CityAreaModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}