using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.PakClassified.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementCategoryController : ControllerBase
    {
        private readonly IAdvertisementCategoryService _advertisementCategoryService;
        private readonly ILogger<AdvertisementCategoryController> _logger;

        public AdvertisementCategoryController(IAdvertisementCategoryService advertisementCategoryService, ILogger<AdvertisementCategoryController> logger)
        {
            _advertisementCategoryService = advertisementCategoryService;
            _logger = logger;
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAdvertisementCategories()
        {
            _logger.LogInformation("Fetching all Advertisement Categories.");

            var response = (await _advertisementCategoryService.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Advertisement Category Found in Database.");
                return NotFound(new { message = "No Advertisement Category Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Advertisements Category", response.Count());
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Advertisement Category by Id: \"{id}\"", id);

            var response = await _advertisementCategoryService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Advertisement Category was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Advertisement Category was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Advertisement Category: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] AdvertisementCategoryModel request)
        {
            _logger.LogInformation("Creating Advertisement Category: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _advertisementCategoryService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Advertisement Category {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] AdvertisementCategoryModel request, int id)
        {
            _logger.LogInformation("Fetching the Advertisement Category ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _advertisementCategoryService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Category ({id}).", id);
                return NotFound(new { message = $"No Advertisement Category was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Advertisement Category with Id: {id}", response.Id);
            return Ok(response);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Advertisement Category ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _advertisementCategoryService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Category ({id}).", id);
                return NotFound(new { message = $"No Advertisement Category was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Advertisement Category with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
