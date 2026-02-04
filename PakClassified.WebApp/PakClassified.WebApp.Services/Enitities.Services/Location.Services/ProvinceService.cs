using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.Locations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PakClassified.WebApp.DTOs.Location.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b._PakClassified.WebApp.Services.Enitities.Services.Location.Services
{
    public interface IProvinceService
    {
        Task<IEnumerable<ProvinceModel>> GetAllAsync();
        Task<ProvinceModel?> GetByIdAsync(int id);
        Task<ProvinceModel> CreateAsync(ProvinceModel province);
        Task<ProvinceModel?> UpdateAsync(int id, ProvinceModel province);
        Task<ProvinceModel?> DeleteAsync(int id, string username);
    }

    public class ProvinceService : IProvinceService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;    // Dependency Injection of the Mapper
        public ProvinceService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Services Implementation of ProvinceService
        public async Task<ProvinceModel> CreateAsync(ProvinceModel request)          // Create New Province
        {
            try
            {
                var province = _mapper.Map<Province>(request);
                //province.Name is Coming From Controller
                //province.LastModifiedDate is Coming From Controller
                //province.CountryId is Coming From Controller

                province.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                province.IsActive = true;
                province.Cities = new List<City>();

                //province.CreatedBy is extracted from Payload of JWT Token in Controller
                province.CreatedDate = DateTime.Now;

                await _dbContext.Provinces.AddAsync(province);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<ProvinceModel>(province);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ProvinceModel>> GetAllAsync()     // GetAll Active Provinces
        {
            try
            {
                return _mapper.Map<IEnumerable<ProvinceModel>>(await _dbContext.Provinces.Where(p => p.IsActive).OrderByDescending(p => p.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ProvinceModel?> GetByIdAsync(int id)      // Get Province By Id
        {
            try
            {
                return _mapper.Map<ProvinceModel>(await _dbContext.Provinces.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ProvinceModel?> UpdateAsync(int id, ProvinceModel province)      // Update Province By Id
        {
            try
            {
                var found = _mapper.Map<Province>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = province.Name;
                    found.CountryId = province.CountryId;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = province.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Provinces.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<ProvinceModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ProvinceModel?> DeleteAsync(int id, string username)           // Soft Delete Province By Id
        {
            try
            {
                var found = _mapper.Map<Province>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Provinces.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<ProvinceModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}