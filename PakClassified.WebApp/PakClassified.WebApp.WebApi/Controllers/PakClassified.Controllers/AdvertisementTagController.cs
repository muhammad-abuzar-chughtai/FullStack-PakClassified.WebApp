using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.PakClassified.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementTagController : ControllerBase
    {
        private readonly IAdvertisementTagService _advertisementTagService;
        private readonly ILogger<AdvertisementTagController> _logger;

        public AdvertisementTagController(IAdvertisementTagService advertisementTagService, ILogger<AdvertisementTagController> logger)
        {
            _advertisementTagService = advertisementTagService;
            _logger = logger;
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAdvertisementTags()
        {
            _logger.LogInformation("Fetching all Advertisement Tags.");

            var response = (await _advertisementTagService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Advertisement Tag Found in Database.");
                return NotFound(new { message = "No Advertisement Tag Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Advertisement Tags", response.Count());
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Advertisement Tag by Id: \"{id}\"", id);

            var response = await _advertisementTagService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Advertisement Tag was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Advertisement Tag was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Advertisement Tag: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] AdvertisementTagModel request)
        {
            _logger.LogInformation("Creating Advertisement Tag: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _advertisementTagService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Advertisement Tag {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] AdvertisementTagModel request, int id)
        {
            _logger.LogInformation("Fetching the Advertisement Tag ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _advertisementTagService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Tag ({id}).", id);
                return NotFound(new { message = $"No Advertisement Tag was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Advertisement Tag with Id: {id}", response.Id);
            return Ok(response);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Advertisement Tag ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _advertisementTagService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Tag ({id}).", id);
                return NotFound(new { message = $"No Advertisement Tag was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Advertisement Tag with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
