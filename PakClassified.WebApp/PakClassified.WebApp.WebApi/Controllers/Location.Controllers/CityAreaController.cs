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
    public class CityAreaController : ControllerBase
    {
        private readonly ICityAreaService _cityAreaService;
        private readonly ILogger<CityAreaController> _logger;

        public CityAreaController(ICityAreaService cityAreaService, ILogger<CityAreaController> logger)
        {
            _cityAreaService = cityAreaService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCityAreas()
        {
            _logger.LogInformation("Fetching all City Areas.");

            var response = (await _cityAreaService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No City Area Found in Database.");
                return NotFound(new { message = "No City Area Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} City Areas", response.Count());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching City Area by Id: \"{id}\"", id);

            var response = await _cityAreaService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No City Area was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No City Area was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive City Area: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] CityAreaModel request)
        {
            _logger.LogInformation("Creating city area: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _cityAreaService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a City Area {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] CityAreaModel request, int id)
        {
            _logger.LogInformation("Fetching the City Area ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _cityAreaService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the City Area ({id}).", id);
                return NotFound(new { message = $"No City Area was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated City Area with Id: {id}", response.Id);
            return Ok(response);
        }


        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the City Area ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _cityAreaService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the City Area ({id}).", id);
                return NotFound(new { message = $"No City Area was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted City Area with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
