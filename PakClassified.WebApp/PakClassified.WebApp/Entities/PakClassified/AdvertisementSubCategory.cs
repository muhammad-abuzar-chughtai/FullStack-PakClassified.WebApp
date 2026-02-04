using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Entities.PakClassified
{
    public class AdvertisementSubCategory : IEntityNamed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        // Association
        public ICollection<Advertisement> Advertisements { get; set; }
        public AdvertisementCategory Category { get; set; }
        public int CategoryId { get; set; }
    }
}
