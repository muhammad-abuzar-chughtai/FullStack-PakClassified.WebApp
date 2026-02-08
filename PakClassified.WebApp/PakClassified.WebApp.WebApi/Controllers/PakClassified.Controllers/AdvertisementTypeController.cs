using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.PakClassified.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementTypeController : ControllerBase
    {
        private readonly IAdvertisementTypeService _advertisementTypeService;
        private readonly ILogger<AdvertisementTypeController> _logger;

        public AdvertisementTypeController(IAdvertisementTypeService advertisementTypeService, ILogger<AdvertisementTypeController> logger)
        {
            _advertisementTypeService = advertisementTypeService;
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAdvertisementTypes()
        {
            _logger.LogInformation("Fetching all Advertisement Types.");

            var response = (await _advertisementTypeService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Advertisement Type Found in Database.");
                return NotFound(new { message = "No Advertisement Type Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Advertisement Types", response.Count());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Advertisement Type by Id: \"{id}\"", id);

            var response = await _advertisementTypeService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Advertisement Type was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Advertisement Type was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Advertisement Type: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] AdvertisementTypeModel request)
        {
            _logger.LogInformation("Creating Advertisement Type: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _advertisementTypeService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Advertisement Type {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] AdvertisementTypeModel request, int id)
        {
            _logger.LogInformation("Fetching the Advertisement Type ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _advertisementTypeService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Type ({id}).", id);
                return NotFound(new { message = $"No Advertisement Type was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Advertisement Type with Id: {id}", response.Id);
            return Ok(response);
        }


        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Advertisement Type ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _advertisementTypeService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Type ({id}).", id);
                return NotFound(new { message = $"No Advertisement Type was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Advertisement Type with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
