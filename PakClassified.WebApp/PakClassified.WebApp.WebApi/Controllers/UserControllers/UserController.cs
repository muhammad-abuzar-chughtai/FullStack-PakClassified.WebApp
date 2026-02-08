using b._PakClassified.WebApp.Services.Enitities.Services.UserServices;
using c._PakClassified.WebApp.DTOs.User.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.User.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService UserService, ILogger<UserController> logger)
        {
            _userService = UserService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching all Users.");

            var response = (await _userService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No User Found in Database.");
                return NotFound(new { message = "No User Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Users", response.Count());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching User by Id: \"{id}\"", id);

            var response = await _userService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No User was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No User was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive User: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromForm] UserCreateDto request)
        {
            _logger.LogInformation("Creating UserModel from UserCreateDto");
            var modelrequest = new UserModel
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                ContactNo = request.ContactNo,
                DOB = request.DOB,
                SecQues = request.SecQues,
                SecAns = request.SecAns,
                RoleId = request.RoleId
            };
            string pass = request.Pass;


            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            modelrequest.CreatedBy = "Login";

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

            var response = (await _userService.CreateAsync(modelrequest, pass));

            _logger.LogInformation("Successfully Created a User {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromForm] UserCreateDto request, int id)
        {
            _logger.LogInformation("Creating UserModel from UserCreateDto");
            var modelrequest = new UserModel
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                ContactNo = request.ContactNo,
                DOB = request.DOB,
                SecQues = request.SecQues,
                SecAns = request.SecAns,
                CreatedBy = request.CreatedBy,
                RoleId = request.RoleId
            };

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            modelrequest.LastModifiedBy = LoginUser;

            _logger.LogInformation("MemoryStream Converting IFormFile to Byte[]....");

            if (request.ProfilePic != null && request.ProfilePic.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.ProfilePic.CopyToAsync(memoryStream);
                    modelrequest.ProfilePic = memoryStream.ToArray();
                }
            }



            _logger.LogInformation("Fetching the User ({id}) to Update.", id);


            var response = await _userService.UpdateAsync(id, modelrequest);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the User ({id}).", id);
                return NotFound(new { message = $"No User was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated User with Id: {id}", response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the User ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _userService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the User ({id}).", id);
                return NotFound(new { message = $"No User was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted User with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
