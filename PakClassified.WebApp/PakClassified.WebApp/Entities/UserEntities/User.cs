using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Entities.UserEntities
{
    public class User : IEntityNamed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] ProfilePic { get; set; }
        public int ContactNo { get; set; }
        public DateTime DOB { get; set; }
        public string? SecQues { get; set; }
        public string? SecAns { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        //Associations
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}
