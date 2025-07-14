using Microsoft.AspNetCore.Mvc;
using Vacation.Business.DTOs;
using Vacation.Business.Services.DepartmentServices;
namespace VacationRequestSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving departments", error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var DepartmentDto = await _departmentService.CreateDepartmentAsync(createDto);
                return CreatedAtAction(nameof(GetDepartment), new { id = DepartmentDto.Id }, DepartmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while Creating your Department.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            try
            {
                var department = await _departmentService.GetDepartmenAsync(id);
                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }
                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving Department", error = ex.Message });
            }
        }




        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int id, [FromBody] string name)
        {
            try
            {
                var updatedRequest = await _departmentService.UpdateDepartmentAsync(id, name);

                return Ok(updatedRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the Department", error = ex.Message });
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            try
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "Department not found" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while Deleting the Department", error = ex.Message });
            }

        }
    }
} 