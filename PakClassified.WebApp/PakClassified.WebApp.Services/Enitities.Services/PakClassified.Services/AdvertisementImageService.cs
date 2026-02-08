using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services
{
    public interface IAdvertisementImageService
    {
        Task<IEnumerable<AdvertisementImageModel>> GetAllAsync(int advertisementId);
        Task<AdvertisementImageModel?> GetByIdAsync(int id);
        Task<AdvertisementImageModel> CreateAsync(AdvertisementImageModel advertisementImage);
        Task<AdvertisementImageModel?> UpdateAsync(int id, AdvertisementImageModel advertisementImage);
        Task<AdvertisementImageModel?> DeleteAsync(int id, string username);
    }
    public class AdvertisementImageService : IAdvertisementImageService
    {
        private readonly AppDBContext _dbContext;    // Dependency Injection of the DbContext
        private readonly IMapper _mapper;    // Dependency Injection of the Mapper
        public AdvertisementImageService(AppDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Services Implementation of AdvertisementImageService
        public async Task<AdvertisementImageModel> CreateAsync(AdvertisementImageModel request)
        {
            try
            {
                var advertisementImage = _mapper.Map<AdvertisementImage>(request);
                // Name is Coming From Controller
                // Content is Coming From Controller
                // Caption is Coming From Controller
                // AdvertisementId is Coming From Controller

                advertisementImage.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                advertisementImage.IsActive = true;

                // CreatedBy is extracted from Payload of JWT Token in Controller
                advertisementImage.CreatedDate = DateTime.Now;

                await _dbContext.AdvertisementImages.AddAsync(advertisementImage);
                await _dbContext.SaveChangesAsync();


                return _mapper.Map<AdvertisementImageModel>(advertisementImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AdvertisementImageModel>> GetAllAsync(int advertisementId)     // GetAll Active AdvertisementImages
        {
            try
            {
                return _mapper.Map<IEnumerable<AdvertisementImageModel>>(await _dbContext.AdvertisementImages.Where(c => c.IsActive && c.AdvertisementId == advertisementId).OrderByDescending(c => c.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementImageModel?> GetByIdAsync(int id)     // GetAll Active AdvertisementImages
        {
            try
            {
                return _mapper.Map<AdvertisementImageModel>(await _dbContext.AdvertisementImages.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private  async Task<AdvertisementImage?> GetById(int id)     // GetAll Active AdvertisementImages
        {
            try
            {
                return await _dbContext.AdvertisementImages.Where(c => c.IsActive && c.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementImageModel?> UpdateAsync(int id, AdvertisementImageModel advertisementImage)      // Update AdvertisementImage
        {
            try
            {
                var found = await GetById(id);
                if (found != null)
                {
                    found.Name = advertisementImage.Name;
                    found.Content = advertisementImage.Content;
                    found.Caption = advertisementImage.Caption;
                    found.AdvertisementId = advertisementImage.AdvertisementId;

                    // LastModifiedBy is extracted from Payload of JWT Token in Controller
                    found.LastModifiedBy = advertisementImage.LastModifiedBy;
                    found.LastModifiedDate = DateTime.Now;

                    //_dbContext.AdvertisementImages.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementImageModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AdvertisementImageModel?> DeleteAsync(int id, string username)      // Soft Delete AdvertisementImage
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

                    //_dbContext.AdvertisementImages.Update(found);
                    await _dbContext.SaveChangesAsync();
                }
                return _mapper.Map<AdvertisementImageModel>(found);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }


}
