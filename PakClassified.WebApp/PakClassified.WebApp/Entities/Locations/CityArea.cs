using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Entities.Locations
{
    public class CityArea: IEntityNamed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        //Associations
        public ICollection<Advertisement> Advertisements { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
