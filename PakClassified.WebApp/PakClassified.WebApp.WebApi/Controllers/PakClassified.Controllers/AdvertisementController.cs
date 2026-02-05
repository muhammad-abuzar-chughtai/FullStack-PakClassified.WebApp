using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.PakClassified.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly ILogger<AdvertisementController> _logger;

        public AdvertisementController(IAdvertisementService advertisementService, ILogger<AdvertisementController> logger)
        {
            _advertisementService = advertisementService;
            _logger = logger;
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAdvertisements()
        {
            _logger.LogInformation("Fetching all Advertisements.");

            var response = (await _advertisementService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Advertisement Found in Database.");
                return NotFound(new { message = "No Advertisement Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Advertisements", response.Count());
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Advertisement by Id: \"{id}\"", id);

            var response = await _advertisementService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Advertisement was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Advertisement was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Advertisement: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] AdvertisementModel request)
        {
            _logger.LogInformation("Creating Advertisement: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _advertisementService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Advertisement {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] AdvertisementModel request, int id)
        {
            _logger.LogInformation("Fetching the Advertisement ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _advertisementService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement ({id}).", id);
                return NotFound(new { message = $"No Advertisement was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Advertisement with Id: {id}", response.Id);
            return Ok(response);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Advertisement ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _advertisementService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement ({id}).", id);
                return NotFound(new { message = $"No Advertisement was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Advertisement with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
