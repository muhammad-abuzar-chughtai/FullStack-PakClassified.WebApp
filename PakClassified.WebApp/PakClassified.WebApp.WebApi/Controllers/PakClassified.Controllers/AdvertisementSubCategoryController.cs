using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.PakClassified.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementSubCategoryController : ControllerBase
    {
        private readonly IAdvertisementSubCategoryService _advertisementSubCategoryService;
        private readonly ILogger<AdvertisementSubCategoryController> _logger;

        public AdvertisementSubCategoryController(IAdvertisementSubCategoryService advertisementSubCategoryService, ILogger<AdvertisementSubCategoryController> logger)
        {
            _advertisementSubCategoryService = advertisementSubCategoryService;
            _logger = logger;
        }


        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAdvertisementCategories()
        {
            _logger.LogInformation("Fetching all Advertisement Categories.");

            var response = await _advertisementSubCategoryService.GetAllAsync();

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Advertisement Sub Category Found in Database.");
                return NotFound(new { message = "No Advertisement Sub Category Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Advertisements Sub Category", response.Count());
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Advertisement Sub Category by Id: \"{id}\"", id);

            var response = await _advertisementSubCategoryService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Advertisement Sub Category was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Advertisement Sub Category was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Advertisement Sub Category: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] AdvertisementSubCategoryModel request)
        {
            _logger.LogInformation("Creating Advertisement Sub Category: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _advertisementSubCategoryService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Advertisement Sub Category {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] AdvertisementSubCategoryModel request, int id)
        {
            _logger.LogInformation("Fetching the Advertisement Sub Category ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _advertisementSubCategoryService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Sub Category ({id}).", id);
                return NotFound(new { message = $"No Advertisement Sub Category was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Advertisement Sub Category with Id: {id}", response.Id);
            return Ok(response);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Advertisement Sub Category ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _advertisementSubCategoryService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Sub Category ({id}).", id);
                return NotFound(new { message = $"No Advertisement Sub Category was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Advertisement Sub Category with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
