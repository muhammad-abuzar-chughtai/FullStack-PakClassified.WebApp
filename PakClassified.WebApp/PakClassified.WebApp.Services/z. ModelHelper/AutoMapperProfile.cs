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

            // ✅ Advertisement → AdvertisementModel
            CreateMap<Advertisement, AdvertisementModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                 .ForMember(dest => dest.TagsId, opt => opt.MapFrom(src => src.Tags
                                                            .Where(t => t.IsActive)
                                                            .Select(t => t.Id)
                                                            .ToList()))
                 .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images
                                                            .Where(i => i.IsActive)
                                                            .Select(i => i.Id)
                                                            .ToList()));
            // ✅ Reverse: AdvertisementModel → Advertisement
            // Tags aur Images auto-handled in UpdateAsync of advertisement
            CreateMap<AdvertisementModel, Advertisement>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

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
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<Role, RoleModel>().ReverseMap();

            #endregion

            #region AuthMapping

            CreateMap<SignupModel, User>();

            #endregion

        }
    }
}
