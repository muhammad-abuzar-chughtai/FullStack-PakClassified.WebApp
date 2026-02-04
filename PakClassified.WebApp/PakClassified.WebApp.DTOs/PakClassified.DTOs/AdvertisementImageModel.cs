using a._PakClassified.WebApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakClassified.WebApp.DTOs.PakClassified.DTOs
{
    public class AdvertisementImageModel : IEntityNamedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string? Caption { get; set; }
        public string CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public int AdvertisementId { get; set; }
    }
}
