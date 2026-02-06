using a._PakClassified.WebApp.Entities.AppDbContext;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using a._PakClassified.WebApp.Entities.Entities.UserEntities;
using AutoMapper;
using Azure.Core;
using b._PakClassified.WebApp.Services.Enitities.Services.UserServices;
using c._PakClassified.WebApp.DTOs.User.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PakClassified.WebApp.DTOs.Auth.DTO;
using PakClassified.WebApp.DTOs.User.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
//using System.Security.Claims.ClaimTypes;
using System.Text;
using System.Threading.Tasks;

namespace b._PakClassified.WebApp.Services.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthResult> SignUpAsync(SignupModel model);
        Task<AuthResult> SignInAsync(SigninModel model);

    }
    public class AuthService : IAuthService
    {
        private readonly AppDBContext _dBContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDBContext dBContext, IMapper mapper, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResult> SignUpAsync(SignupModel request)
        {
            try
            {
                bool emailExists = await _dBContext.Users.AnyAsync(u => u.Email == request.Email);
                if (emailExists)
                {
                    _logger.LogWarning("User Already Exist");
                    return new AuthResult();
                }

                var user = _mapper.Map<User>(request);
                //Name, Email, Pass, ProfilePic, ContactNo, DOB, SecQues, SecAns, are coming from Controller
                user.Id = 0; // Ensure the ID is zero for new entity [(EFcore will adjust the Id with Intelisense)]
                user.IsActive = true;
                user.Advertisements = new List<Advertisement>();

                user.RoleId = 1;

                //country.CreatedBy is extracted from Payload of JWT Token in Controller
                user.CreatedDate = DateTime.Now;

                await _dBContext.Users.AddAsync(user);
                await _dBContext.SaveChangesAsync();

                await _dBContext.Entry(user).Reference(u => u.Role).LoadAsync();

                _logger.LogInformation("Issueing Token.");
                string token = IssueToken(user);

                AuthResult authResult = new AuthResult
                {
                    userModel = _mapper.Map<UserModel>(user),
                    Token = token
                };
                return authResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<AuthResult> SignInAsync(SigninModel model)
        {
            var user = await _dBContext.Users.Where(u => u.IsActive && u.Email == model.Email && u.Password == model.Pass).Include(u => u.Role).FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("Invalid Credentials, User Not Found.");
                return new AuthResult();
            }

            _logger.LogInformation("Issueing Token.");
            string token = IssueToken(user);

            AuthResult authResult = new AuthResult
            {
                userModel = _mapper.Map<UserModel>(user),
                Token = token
            };

            return authResult;
        }

        private string IssueToken(User user)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var Claims = new List<Claim>
            {
                new Claim("Myapp_User_Id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            _logger.LogInformation("Assembling Token..");
            var token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: Claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials
                );


            _logger.LogInformation("Issuing...");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
