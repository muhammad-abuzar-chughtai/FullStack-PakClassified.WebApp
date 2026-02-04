using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Entities.PakClassified
{
    public class AdvertisementTag: IEntityNamed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? NoOfSearch { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        // Association
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
