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
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly ILogger<CityController> _logger;

        public CityController(ICityService cityService, ILogger<CityController> logger)
        {
            _cityService = cityService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCities()
        {
            _logger.LogInformation("Fetching all Cities.");

            var response = (await _cityService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No City Found in Database.");
                return NotFound(new { message = "No City Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Cities", response.Count());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching City by Id: \"{id}\"", id);

            var response = await _cityService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No City was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No City was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive City: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] CityModel request)
        {
            _logger.LogInformation("Creating city: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _cityService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a City {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] CityModel request, int id)
        {
            _logger.LogInformation("Fetching the City ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _cityService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the City ({id}).", id);
                return NotFound(new { message = $"No City was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated City with Id: {id}", response.Id);
            return Ok(response);
        }


        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the City ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _cityService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the City ({id}).", id);
                return NotFound(new { message = $"No City was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted City with Id: {id}", response.Id);
            return Ok(response);
        }









    }
}
