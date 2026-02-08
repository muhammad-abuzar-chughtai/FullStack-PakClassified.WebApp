using AutoMapper;
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
    public class CountryController : ControllerBase
    {

        private readonly ICountryService _countryServices;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryServices, ILogger<CountryController> logger)
        {
            _countryServices = countryServices;
            _logger = logger;
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCountries()
        {
            _logger.LogInformation("Fetching all Countries.");

            var response = (await _countryServices.GetAllAsync());

            if (response.Count() <= 0)
            {
                _logger.LogWarning("No Country Found in Database.");
                return NotFound(new { message = "No Country Found." });
            }

            _logger.LogInformation("Successfully retrived {Count} Countries", response.Count());
            return Ok(response);
        }

        [Authorize(Roles = "Admin, Manager, Head")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching Country by Id: \"{id}\"", id);

            var response = await _countryServices.GetByIdAsync(id);
            if (response == null)
            {
                _logger.LogWarning("No Country was found by this Id: \"{id}\"", id);
                return NotFound(new { message = $"No Country was found by this id {id}" });
            }

            _logger.LogInformation("Successfully retrive Country: {name} with id: {id}", response.Name, response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] CountryModel request)
        {
            _logger.LogInformation("Creating country: {Name}", request.Name);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);     // Login User, Extracted from Token
            request.CreatedBy = LoginUser;

            var response = (await _countryServices.CreateAsync(request));

            _logger.LogInformation("Successfully Created a Country {Name}", response.Name);

            return Created(string.Empty, response);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] CountryModel request, int id)
        {
            _logger.LogInformation("Fetching the Country ({id}) to Update.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name); // Login User, Extracted from Token
            request.LastModifiedBy = LoginUser;

            var response = await _countryServices.UpdateAsync(id, request);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Country ({id}).", id);
                return NotFound(new { message = $"No Country was found with Id: {id}" });
            }

            _logger.LogInformation("Successfully Updated Country with Id: {id}", response.Id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching the Country ({id}) to Delete.", id);

            string LoginUser = User.FindFirstValue(ClaimTypes.Name);          // Login User, Extracted from Token

            var response = await _countryServices.DeleteAsync(id, LoginUser);

            if (response == null)
            {
                _logger.LogWarning("Failed to Fetch the Country ({id}).", id);
                return NotFound(new { message = $"No Country was found with Id: {id}" });
            }
            _logger.LogInformation("Successfully Deleted Country with Id: {id}", response.Id);
            return Ok(response);
        }






    }
}
