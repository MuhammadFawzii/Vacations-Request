using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using Vacation.Business.DTOs;

namespace Vacation.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployeesController> _logger;
        public EmployeesController(IHttpClientFactory httpClientFactory, ILogger<EmployeesController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Employees");
                response.EnsureSuccessStatusCode();
                var EmployeesDto = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>() ?? Enumerable.Empty<EmployeeDto>();

                return View(EmployeesDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Employees.");
                TempData["error"] = "An error occurred while retrieving Employees.";
                return View(new List<EmployeeDto>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetDepartments();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(EmployeeDto createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(createDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/Employees", content);
                    response.EnsureSuccessStatusCode();
                    TempData["success"] = "Employee created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the Employees.");
                }
            }
            await GetDepartments();
            return View(createDto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Employees/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                response.EnsureSuccessStatusCode();
                var employeeDto = await response.Content.ReadFromJsonAsync<EmployeeDto>();
                await GetDepartments();
                return View(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the employee for editing.");
                TempData["error"] = "An error occurred while retrieving the employee for editing.";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Employees/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                response.EnsureSuccessStatusCode();
                var employeeDto = await response.Content.ReadFromJsonAsync<EmployeeDto>();
                return View(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employee for details.");
                TempData["error"] = "An error occurred while retrieving employee for details.";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, EmployeeDto employeeDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(employeeDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/employees/{id}", content);

                response.EnsureSuccessStatusCode();
                TempData["success"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating Employee.");

            }
            await GetDepartments();
            return View(employeeDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/employees/{id}");
                response.EnsureSuccessStatusCode();
                var employeeDto = await response.Content.ReadFromJsonAsync<EmployeeDto>();
                return View(employeeDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleting employee.");
                TempData["success"] = "An error occurred while Deleting employee.";
                return View(new List<EmployeeDto>());
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/employees/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Employee deleted successfully!";
                }
                else
                {
                    TempData["error"] = "Employee not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while deleting the Employee.";
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task GetDepartments()
        {
            var response = await _httpClient.GetAsync("api/Departments");
            response.EnsureSuccessStatusCode();
            var departments = await response.Content.ReadFromJsonAsync<IEnumerable<DepartmentDto>>() ?? Enumerable.Empty<DepartmentDto>();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
        }
        private async Task GetEmployees()
        {
            var empResponse = await _httpClient.GetAsync("api/Employees");
            empResponse.EnsureSuccessStatusCode();
            var employeeDtos = await empResponse.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>() ?? Enumerable.Empty<EmployeeDto>();
            var deptResponse = await _httpClient.GetAsync("api/Departments");
            deptResponse.EnsureSuccessStatusCode();
            var departments = await deptResponse.Content.ReadFromJsonAsync<IEnumerable<DepartmentDto>>() ?? Enumerable.Empty<DepartmentDto>();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
        }
    }
}
