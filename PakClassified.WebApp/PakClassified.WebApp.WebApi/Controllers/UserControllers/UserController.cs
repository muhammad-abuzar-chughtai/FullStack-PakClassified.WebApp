using b._PakClassified.WebApp.Services.Enitities.Services.UserServices;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] UserModel request)
        {
            _logger.LogInformation("Creating User: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            // IFormFile → Byte[]

            var response = (await _userService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a User {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] UserModel request, int id)
        {
            _logger.LogInformation("Fetching the User ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;


            // IFormFile → Byte[]


            var response = await _userService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the User ({id}).", id);
                return NotFound(new { message = $"No User was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated User with Id: {id}", response.Id);
            return Ok(response);
        }


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
