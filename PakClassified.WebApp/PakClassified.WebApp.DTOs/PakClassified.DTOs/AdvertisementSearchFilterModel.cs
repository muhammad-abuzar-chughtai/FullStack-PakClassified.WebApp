using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakClassified.WebApp.DTOs.PakClassified.DTOs
{
    public class AdvertisementSearchFilterModel
    {
        public int? CityAreaId { get; set; }
        public int? PostedById { get; set; }
        public int? StatusId { get; set; }
        public int? TypeId { get; set; }
        public int? SubCategoryId { get; set; }
        public ICollection<int>? TagIds { get; set; }
    }
}
