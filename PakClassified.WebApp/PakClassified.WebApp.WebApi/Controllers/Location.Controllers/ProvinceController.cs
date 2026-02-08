using b._PakClassified.WebApp.Services.Enitities.Services.Location.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.Location.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.Location.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;
        private readonly ILogger<ProvinceController> _logger;

        public ProvinceController(IProvinceService ProvinceService, ILogger<ProvinceController> logger)
        {
            _provinceService = ProvinceService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllProvinces()
        {
            _logger.LogInformation("Fetching all Cities.");
            
            var response = (await _provinceService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Province Found in Database.");
                return NotFound(new { message = "No Province Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Provinces", response.Count());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Province by Id: \"{id}\"", id);

            var response = await _provinceService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Province was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Province was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Province: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] ProvinceModel request)
        {
            _logger.LogInformation("Creating Province: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _provinceService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Province {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] ProvinceModel request, int id)
        {
            _logger.LogInformation("Fetching the Province ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _provinceService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Province ({id}).", id);
                return NotFound(new { message = $"No Province was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Province with Id: {id}", response.Id);
            return Ok(response);
        }


        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Province ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _provinceService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Province ({id}).", id);
                return NotFound(new { message = $"No Province was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Province with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
