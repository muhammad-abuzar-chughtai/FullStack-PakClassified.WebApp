using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.Locations;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
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

namespace b._PakClassified.WebApp.Services.Enitities.Services.UserServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<UserModel?> GetByIdAsync(int id);
        Task<UserModel> CreateAsync(UserModel user);
        Task<UserModel?> UpdateAsync(int id, UserModel user);
        Task<UserModel?> DeleteAsync(int id, string username);
    }
    public class UserService : IUserService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;    // Dependency Injection of the Mapper
        public UserService(AppDBContext appDBContext, IMapper mapper)
        {
            _dbContext = appDBContext;
            _mapper = mapper;
        }

        // Services Implementation of UserService
        public async Task<UserModel?> CreateAsync(UserModel request)           // Create New User
        {
            try
            {
                var user = _mapper.Map<User>(request);
                //Name, Email, ProfilePic, ContactNo, DOB, SecQues, SecAns, are coming from Controller
                user.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                user.IsActive = true;
                user.Advertisements = new List<Advertisement>();

                //country.CreatedBy is extracted from Payload of JWT Token in Controller
                user.CreatedDate = DateTime.Now;

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<UserModel>(user);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()      // GetAll Active Users
        {
            try
            {
                return _mapper.Map<IEnumerable<UserModel>>(await _dbContext.Users.Where(u => u.IsActive).OrderByDescending(u => u.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<UserModel?> GetByIdAsync(int id)      // Get User By Id
        {
            try
            {
                return _mapper.Map<UserModel>(await _dbContext.Users.Where(u => u.IsActive && u.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<UserModel?> UpdateAsync(int id, UserModel user)      // Update User By Id
        {
            try
            {
                var found = _mapper.Map<User>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.Name = user.Name;
                    found.Email = user.Email;
                    found.ContactNo = user.ContactNo;
                    found.ProfilePic = user.ProfilePic;
                    found.DOB = user.DOB;
                    found.SecQues = user.SecQues;
                    found.SecAns = user.SecAns;

                    found.RoleId = user.RoleId;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = user.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Users.Update(found);
                    await _dbContext.SaveChangesAsync();
                }

                return _mapper.Map<UserModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<UserModel?> DeleteAsync(int id, string username)      // Soft Delete User By Id
        {
            try
            {
                var found = _mapper.Map<User>(await GetByIdAsync(id));
                if (found != null)
                {
                    found.IsActive = false;

                    //country.LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = username;
                    found.LastModifiedDate = DateTime.Now;

                    _dbContext.Users.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<UserModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
