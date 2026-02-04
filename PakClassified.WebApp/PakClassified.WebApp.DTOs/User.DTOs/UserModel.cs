using a._PakClassified.WebApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakClassified.WebApp.DTOs.User.DTOs
{
    public class UserModel: IEntityNamedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public byte[] ProfilePic { get; set; }
        public int ContactNo { get; set; }
        public DateTime DOB{ get; set; }
        public string? SecQues { get; set; }
        public string? SecAns { get; set; }

        public string CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }

        public int RoleId { get; set; }
    }
}
