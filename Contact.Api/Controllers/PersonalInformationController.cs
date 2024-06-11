using Contact.Infrastructure.Interfaces;
using Contact.Shared.Custom;
using Contact.Shared.Dto;
using Contact.Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalInformationController : ControllerBase
    {
        private readonly IPersonalInformationService _personalInformationService;

        public PersonalInformationController(IPersonalInformationService personalInformationService)
        {
            _personalInformationService = personalInformationService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> AddPersonalInformation([FromForm] PersonalInformationDto personalInformation)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _personalInformationService.AddPersonalInformationAsync(personalInformation);
                return Created(string.Empty, new { message = "Personal information has been created" });
            }
            catch (NotFoundErrorException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (ConflictErrorException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UpdatePersonalInformation(int id, [FromForm] PersonalInformationDto personalInformation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _personalInformationService.UpdatePersonalInformationAsync(id, personalInformation);
                return Ok(new { message = "Personal information has been updated" });
            }
            catch (NotFoundErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> DeletePersonalInformation(int id)
        {
            try
            {
                await _personalInformationService.DeletePersonalInformationAsync(id);
                return Ok(new { message = "Personal information has been deleted" });
            }
            catch (NotFoundErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonalInformation))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetPersonalInformation(int id)
        {
            try
            {
                var personalInformation = await _personalInformationService.GetPersonalInformationAsync(id);
                var imagePath = personalInformation.ImageUrl;
                string? imageUrl = null;
                if (!string.IsNullOrEmpty(personalInformation.ImageUrl))
                {
                    imageUrl = Url.Action("GetImage", new { id });
                }

                return Ok(new PersonalInformationResponseDto { PersonalInformation = personalInformation, ImageUrl = imageUrl });
            }
            catch (NotFoundErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetImage/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var personalInformation = await _personalInformationService.GetPersonalInformationAsync(id);
                if (personalInformation == null || string.IsNullOrEmpty(personalInformation.ImageUrl))
                {
                    return NotFound("Image not found");
                }

                var imagePath = personalInformation.ImageUrl;
                if (!System.IO.File.Exists(imagePath))
                {
                    return NotFound("Image file not found");
                }

                var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, personalInformation.ImageContentType!, $"fromapi_image");
            }
            catch (NotFoundErrorException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GeAllPersonalInformations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PersonalInformation>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetAllPersonalInformation()
        {
            try
            {
                var personalInformations = await _personalInformationService.GetAllPersonalInformationAsync();
                return Ok(personalInformations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
