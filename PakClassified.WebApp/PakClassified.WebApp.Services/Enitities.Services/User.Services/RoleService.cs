using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.Locations;
using a._PakClassified.WebApp.Entities.Entities.UserEntities;
using a._PakClassified.WebApp.Entities.Mapping.UserEntities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PakClassified.WebApp.DTOs.User.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b._PakClassified.WebApp.Services.Enitities.Services.RoleServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleModel>> GetAllAsync();
        Task<RoleModel> GetByIdAsync(int id);
        Task<RoleModel> CreateAsync(RoleModel role);
        Task<RoleModel> UpdateAsync(int id, RoleModel role);
        Task<RoleModel> DeleteAsync(int id, string username);


    }
    public class RoleService : IRoleService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;    // Dependency Injection of the Mapper
        public RoleService(AppDBContext appDBContext, IMapper mapper)
        {
            _dbContext = appDBContext;
            _mapper = mapper;
        }

        // Services Implementation of RoleService
        public async Task<RoleModel> CreateAsync(RoleModel request)       // Create New Role
        {
            try
            {
                var role = _mapper.Map<Role>(request);
                //role.Name is Coming From Controller
                //role.LastModifiedDate is Coming From Controller

                role.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                role.IsActive = true;
                role.Users = new List<User>();

                //role.CreatedBy is extracted from Payload of JWT Token in Controller
                role.CreatedDate = DateTime.Now;

                await _dbContext.Roles.AddAsync(role);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<RoleModel>(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<RoleModel>> GetAllAsync()      // GetAll Active Roles
        {
            try
            {
                return _mapper.Map<IEnumerable<RoleModel>>(await _dbContext.Roles.Where(r => r.IsActive).OrderByDescending(R => R.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<RoleModel?> GetByIdAsync(int id)         // Get Country by Id
        {
            try
            {
                return _mapper.Map<RoleModel>(await _dbContext.Roles.Where(r => r.IsActive && r.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<RoleModel?> UpdateAsync(int id, RoleModel role)       // Update Country by Id
        {
            try
            {
                var found = _mapper.Map<Role>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = role.Name;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = role.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Roles.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<RoleModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<RoleModel?> DeleteAsync(int id, string username)       // Soft Delete Role by Id
        {
            try
            {
                var found = _mapper.Map<Role>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;
                   
                    //role.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;
                   
                    _dbContext.Roles.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<RoleModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
