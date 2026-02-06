using PakClassified.WebApp.DTOs.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c._PakClassified.WebApp.DTOs.User.DTOs
{
    public class AuthResult
    {
        public UserModel userModel { get; set; } 
        public string Token { get; set; } 

    }
}
