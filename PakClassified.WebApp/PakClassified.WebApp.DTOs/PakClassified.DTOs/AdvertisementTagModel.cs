using a._PakClassified.WebApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakClassified.WebApp.DTOs.PakClassified.DTOs
{
    public class AdvertisementTagModel: IEntityNamedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NoOfSearch { get; set; }
        public string CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
