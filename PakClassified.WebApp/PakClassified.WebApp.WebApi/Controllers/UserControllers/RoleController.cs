using b._PakClassified.WebApp.Services.Enitities.Services.RoleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.User.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService RoleService, ILogger<RoleController> logger)
        {
            _roleService = RoleService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRoles()
        {
            _logger.LogInformation("Fetching all Roles.");

            var response = (await _roleService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Role Found in Database.");
                return NotFound(new { message = "No Role Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Roles", response.Count());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Role by Id: \"{id}\"", id);

            var response = await _roleService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Role was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Role was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Role: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] RoleModel request)
        {
            _logger.LogInformation("Creating Role: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _roleService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Role {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] RoleModel request, int id)
        {
            _logger.LogInformation("Fetching the Role ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _roleService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Role ({id}).", id);
                return NotFound(new { message = $"No Role was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Role with Id: {id}", response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Role ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _roleService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Role ({id}).", id);
                return NotFound(new { message = $"No Role was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Role with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
