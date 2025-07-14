using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vacation.Business.DTOs;
using Vacation.Business.Services;
using Vacation.Business.Services.EmployeeServices;

namespace Vacation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving employees", error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var employeedto = await _employeeService.CreateEmployeeAsync(createDto);
                return CreatedAtAction(nameof(GetEmployee), new { id = employeedto.Id }, employeedto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while Creating your employee.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var Employee = await _employeeService.GetEmployeeAsync(id);
                if (Employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }
                return Ok(Employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving Employee", error = ex.Message });
            }
        }




        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id,EmployeeDto dto)
        {
            try
            {
                var updatedEmployee= await _employeeService.UpdateEmployeeAsync(id, dto);

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the Employee", error = ex.Message });
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var deleted = await _employeeService.DeleteEmployeeAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "Employee not found" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while Deleting the Employee", error = ex.Message });
            }

        }
    }
}
