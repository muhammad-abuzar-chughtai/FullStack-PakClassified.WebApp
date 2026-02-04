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
    public interface ICountryService
    {
        Task<IEnumerable<CountryModel>> GetAllAsync();
        Task<CountryModel?> GetByIdAsync(int id);
        Task<CountryModel> CreateAsync(CountryModel request);
        Task<CountryModel?> UpdateAsync(int id, CountryModel request);
        Task<CountryModel?> DeleteAsync(int id, string username);
    }

    public class CountryService : ICountryService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;    // Dependency Injection of the Mapper
        public CountryService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        // Services Implementation of CountryService
        public async Task<CountryModel> CreateAsync(CountryModel request)          // Create New Country
        {
            try
            {
                var country = _mapper.Map<Country>(request);
                //country.Name is Coming From Controller
                //country.LastModifiedDate is Coming From Controller

                country.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                country.IsActive = true;
                country.Provinces = new List<Province>();

                //country.CreatedBy is extracted from Payload of JWT Token in Controller
                country.CreatedDate = DateTime.Now;

                await _dbContext.Countries.AddAsync(country);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<CountryModel>(country);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CountryModel>> GetAllAsync()     // GetAll Active Countries
        {
            try
            {
                return _mapper.Map<IEnumerable<CountryModel>>(await _dbContext.Countries.Where(c => c.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CountryModel> GetByIdAsync(int id)          // Get Country By Id
        {
            try
            {
                return _mapper.Map<CountryModel>(await _dbContext.Countries.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CountryModel?> UpdateAsync(int id, CountryModel country)    // Update Country By Id
        {
            try
            {
                var found = _mapper.Map<Country>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = country.Name;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = country.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Countries.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<CountryModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CountryModel?> DeleteAsync(int id, string username)          // Soft Delete Country By Id
        {
            try
            {
                var found = _mapper.Map<Country>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Countries.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<CountryModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
