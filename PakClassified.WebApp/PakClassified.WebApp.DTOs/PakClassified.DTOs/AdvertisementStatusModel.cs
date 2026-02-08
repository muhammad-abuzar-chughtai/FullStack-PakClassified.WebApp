using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakClassified.WebApp.DTOs.PakClassified.DTOs
{
    public class AdvertisementStatusModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
