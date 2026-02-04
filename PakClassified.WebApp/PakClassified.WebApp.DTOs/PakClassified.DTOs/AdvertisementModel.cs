using a._PakClassified.WebApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakClassified.WebApp.DTOs.PakClassified.DTOs
{
    public class AdvertisementModel: IEntityNamedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Likes { get; set; }
        public DateTime StartsOn { get; set; }
        public DateTime EndsOn { get; set; }
        public string CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public int CityAreaId { get; set; }
        public int PostedById { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public int SubCategoryId { get; set; }
        public ICollection<AdvertisementTagModel> Tags { get; set; }
        public ICollection<AdvertisementImageModel> Images { get; set; }
    }
}
