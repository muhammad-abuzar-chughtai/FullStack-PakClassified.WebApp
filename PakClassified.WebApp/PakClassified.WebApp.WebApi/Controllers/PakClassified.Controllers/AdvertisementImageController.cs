using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakClassified.WebApp.DTOs.PakClassified.DTOs;
using System.Security.Claims;

namespace PakClassified.WebApp.WebApi.Controllers.PakClassified.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementImageController : ControllerBase
    {
        private readonly IAdvertisementImageService _advertisementImageService;
        private readonly ILogger<AdvertisementImageController> _logger;

        public AdvertisementImageController(IAdvertisementImageService advertisementImageService, ILogger<AdvertisementImageController> logger)
        {
            _advertisementImageService = advertisementImageService;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetAll/Advertisement/{id}")]
        public async Task<IActionResult> GetAllAdvertisementImages(int id)
        {
            _logger.LogInformation("Fetching all Advertisement Images.");

            var response = (await _advertisementImageService.GetAllAsync(id));

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Images for this advertisement Found in Database.");
                return NotFound(new { message = "No Image for Advertisement Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Advertisement Images", response.Count());
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Advertisement Image by Id: \"{id}\"", id);

            var response = await _advertisementImageService.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Advertisement Image was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Advertisement Image was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Advertisement Image: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] AdvertisementImageModel request)
        {

            // IFormFile → Byte[]


            _logger.LogInformation("Creating Advertisement Image: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _advertisementImageService.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Advertisement Image {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] AdvertisementImageModel request, int id)
        {

            // IFormFile → Byte[]



            _logger.LogInformation("Fetching the Advertisement Image ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _advertisementImageService.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Image ({id}).", id);
                return NotFound(new { message = $"No Advertisement Image was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Advertisement Image with Id: {id}", response.Id);
            return Ok(response);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Advertisement Image ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _advertisementImageService.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Advertisement Image ({id}).", id);
                return NotFound(new { message = $"No Advertisement Image was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Advertisement Image with Id: {id}", response.Id);
            return Ok(response);
        }
    }
}
