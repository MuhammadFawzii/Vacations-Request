using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vacation.Business.DTOs;
using Vacation.Business.Services.RequestServices;

namespace Vacation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetAll() {

            try
            {
                var requests = await _requestService.GetAllRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { message = "An error occurred while retrieving vacation requests", error = ex.Message });

            }
        }
        [HttpPost]
        public async Task<ActionResult<RequestDto>> CreateRequest(CreateRequestDto createDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var RequestDto = await _requestService.CreateRequestAsync(createDto);
                return CreatedAtAction(nameof(GetRequest), new { id = RequestDto.Id }, RequestDto);
            }   
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while Creating your request.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDto>> GetRequest(int id)
        {
            try
            {
                var request = await _requestService.GetRequestAsync(id);
                if (request == null)
                {
                    return NotFound(new { message = "Vacation request not found" });
                }
                return Ok(request);
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "An error occurred while retrieving the vacation request", error = ex.Message });
            }
        }

        
    

        [HttpPut("{id}/status")]
        public async Task<ActionResult<RequestDto>> UpdateRequestStatus(int id, [FromBody] string status)
        {
            try
            {
                var updatedRequest = await  _requestService.UpdateRequestAsync(id,status);
                
                return Ok(updatedRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the vacation request", error = ex.Message });
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRequest(int id)
        {
            try
            {
                var deleted = await _requestService.DeleteRequestAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "Vacation request not found" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while Deleting the vacation request", error = ex.Message });
            }

        }



    }
}
