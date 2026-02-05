using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.PakClassified.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementStatusController : ControllerBase
    {
        private readonly IAdvertisementStatusService _advertisementStatusService;
        private readonly ILogger<AdvertisementStatusController> _logger;

        public AdvertisementStatusController(IAdvertisementStatusService advertisementStatusService, ILogger<AdvertisementStatusController> logger)
        {
            _advertisementStatusService = advertisementStatusService;
            _logger = logger;
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAdvertisementStatuses()
        {
            _logger.LogInformation("Fetching all Advertisement Statuses.");

            var response = (await _advertisementStatusService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Advertisement Status Found in Database.");
                return NotFound(new { message = "No Advertisement Status Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Advertisement Statuses", response.Count());
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Advertisement Status by Id: \"{id}\"", id);

            var response = await _advertisementStatusService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Advertisement Status was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Advertisement Status was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Advertisement Status: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] AdvertisementStatusModel request)
        {
            _logger.LogInformation("Creating Advertisement Status: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _advertisementStatusService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Advertisement Status {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] AdvertisementStatusModel request, int id)
        {
            _logger.LogInformation("Fetching the Advertisement Status ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _advertisementStatusService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Status ({id}).", id);
                return NotFound(new { message = $"No Advertisement Status was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Advertisement Status with Id: {id}", response.Id);
            return Ok(response);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Advertisement Status ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _advertisementStatusService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Status ({id}).", id);
                return NotFound(new { message = $"No Advertisement Status was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Advertisement Status with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
