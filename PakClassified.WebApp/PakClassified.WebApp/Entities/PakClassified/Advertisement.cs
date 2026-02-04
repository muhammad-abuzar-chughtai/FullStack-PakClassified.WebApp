using a._PakClassified.WebApp.Entities.Entities.Locations;
using a._PakClassified.WebApp.Entities.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Entities.PakClassified
{
    public class Advertisement : IEntityNamed
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
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        // Associations
        public CityArea CityArea { get; set; }
        public int CityAreaId { get; set; }

        public User PostedBy { get; set; }
        public int PostedById { get; set; }
        public AdvertisementStatus Status { get; set; }
        public int StatusId { get; set; }
        public AdvertisementType Type { get; set; }
        public int TypeId { get; set; }
        public AdvertisementSubCategory SubCategory { get; set; }
        public int SubCategoryId { get; set; }
        public ICollection<AdvertisementTag> Tags { get; set; }
        public ICollection<AdvertisementImage> Images { get; set; }
    }
}
