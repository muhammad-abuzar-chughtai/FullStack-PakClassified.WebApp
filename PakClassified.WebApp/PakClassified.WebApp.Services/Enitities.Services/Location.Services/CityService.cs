using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.Locations;
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
    public interface ICityService
    {
        Task<IEnumerable<CityModel>> GetAllAsync();
        Task<CityModel?> GetByIdAsync(int id);
        Task<CityModel> CreateAsync(CityModel request);
        Task<CityModel?> UpdateAsync(int id, CityModel request);
        Task<CityModel?> DeleteAsync(int id, string username);
    }
    public class CityService : ICityService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;    // Dependency Injection of the Mapper
        public CityService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        // Services Implementation of CityService
        public async Task<CityModel> CreateAsync(CityModel request)          // Create New City
        {
            try
            {
                var city = _mapper.Map<City>(request);
                // city.Name is Coming From Controller
                // city.ProvinceId is Coming From Controller

                city.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                city.IsActive = true;
                city.CityAreas = new List<CityArea>();
                
                // city.CreatedBy is extracted from Payload of JWT Token in Controller
                city.CreatedDate = DateTime.Now;
                
                await _dbContext.Cities.AddAsync(city);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<CityModel>(city);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CityModel>> GetAllAsync()     // GetAll Active Cities
        {
            try
            {
                return _mapper.Map<IEnumerable<CityModel>>(await _dbContext.Cities.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CityModel?> GetByIdAsync(int id)      // Get City By Id
        {
            try
            {
                return _mapper.Map<CityModel>(await _dbContext.Cities.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CityModel?> UpdateAsync(int id, CityModel city)      // Update Province By Id
        {
            try
            {
                var found = _mapper.Map<City>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = city.Name;
                    found.ProvinceId = city.ProvinceId;

                    //city.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = city.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Cities.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<CityModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CityModel?> DeleteAsync(int id, string username)           // Soft Delete Province By Id
        {
            try
            {
                var found = _mapper.Map<City>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Cities.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<CityModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
