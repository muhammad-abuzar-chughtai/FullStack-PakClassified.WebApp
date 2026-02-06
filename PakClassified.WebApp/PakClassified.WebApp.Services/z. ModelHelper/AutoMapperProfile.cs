using AutoMapper;
using a._PakClassified.WebApp.Entities.Entities.UserEntities;
using PakClassified.WebApp.DTOs.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a._PakClassified.WebApp.Entities.Entities.Locations;
using PakClassified.WebApp.DTOs.Location.DTOs;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using PakClassified.WebApp.DTOs.Auth.DTO;

namespace b._PakClassified.WebApp.Services.z._ModelHelper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap();
        }

        public void CreateMap()
        {

            #region LocationMapping

            CreateMap<Country, CountryModel>().ReverseMap();
            CreateMap<Province, ProvinceModel>().ReverseMap();
            CreateMap<City, CityModel>().ReverseMap();
            CreateMap<CityArea, CityAreaModel>().ReverseMap();

            #endregion

            #region PakCalssified

            CreateMap<Advertisement, AdvertisementModel>().ReverseMap();
            CreateMap<AdvertisementCategory, AdvertisementCategoryModel>().ReverseMap();
            CreateMap<AdvertisementSubCategory, AdvertisementSubCategoryModel>().ReverseMap();
            CreateMap<AdvertisementStatus, AdvertisementStatusModel>().ReverseMap();
            CreateMap<AdvertisementType, AdvertisementTypeModel>().ReverseMap();
            CreateMap<AdvertisementTag, AdvertisementTagModel>().ReverseMap();
            CreateMap<AdvertisementImage, AdvertisementImageModel>().ReverseMap()
                .ReverseMap()
                .ForMember(dest => dest.ContentFile, opt => opt.Ignore());

            #endregion

            #region UserMapping

            CreateMap<User, UserModel>()
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // Ignore Password when mapping back to User
            CreateMap<Role, RoleModel>().ReverseMap();

            #endregion

            #region AuthMapping

            CreateMap<SignupModel, User>()
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());


            #endregion

        }
    }
}
