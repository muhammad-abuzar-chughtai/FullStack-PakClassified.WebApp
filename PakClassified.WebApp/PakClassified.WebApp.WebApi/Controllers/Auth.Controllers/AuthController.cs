using a._PakClassified.WebApp.Entities.Entities.UserEntities;
using b._PakClassified.WebApp.Services.Auth.Services;
using c._PakClassified.WebApp.DTOs.Auth.DTO;
using c._PakClassified.WebApp.DTOs.User.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.Auth.DTO;
using PakClassified.WebApp.DTOs.User.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup/")]
        public async Task<IActionResult> Signup([FromForm] FSignUpModel request)
        {
            _logger.LogInformation("Creating SignUpModel from FSignUpModel");
            var modelrequest = new SignupModel
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                Password = request.Pass,
                ContactNo = request.ContactNo,
                DOB = request.DOB,
                SecQues = request.SecQues,
                SecAns = request.SecAns,
            };


            modelrequest.CreatedBy = "SignUp";
            _logger.LogInformation("MemoryStream Converting IFormFile to Byte[]....");

            if (request.ProfilePic != null && request.ProfilePic.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.ProfilePic.CopyToAsync(memoryStream);
                    modelrequest.ProfilePic = memoryStream.ToArray();
                }
            }

            _logger.LogInformation("Creating User: {Name}", request.Name);

            var response = await _authService.SignUpAsync(modelrequest);

            if (response == null)
            {
                return BadRequest("User with Same Email already Exist.");
            }

            UserModel responseModel = response.userModel;
            string token = response.Token;

            _logger.LogInformation("Successfully Created a User {Name}", response.userModel.Name);

            return Ok(new { Token = token, Payload = responseModel });

        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Signin/")]
        public async Task<IActionResult> Signin([FromBody] SigninModel request)
        {
           
            var response = await _authService.SignInAsync(request);
            if(response.userModel == null || response.Token == null)
            {
                return Unauthorized("Invalid Credentials, User Not Found.");
            }

            UserModel responseModel = response.userModel;
            string token = response.Token;

            _logger.LogInformation("Successfully Created a User {Name}", response.userModel.Name);

            return Ok(new { Token = token, Payload = responseModel });

        }
    }
}
