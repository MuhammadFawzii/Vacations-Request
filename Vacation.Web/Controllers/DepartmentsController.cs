using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using Vacation.Business.DTOs;

namespace Vacation.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DepartmentsController> _logger;
        public DepartmentsController(IHttpClientFactory httpClientFactory, ILogger<DepartmentsController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Departments");
                response.EnsureSuccessStatusCode();
                var DepartmentDto = await response.Content.ReadFromJsonAsync<IEnumerable<DepartmentDto>>() ?? Enumerable.Empty<DepartmentDto>();

                return View(DepartmentDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Department.");
                TempData["error"] = "An error occurred while retrieving Department.";
                return View(new List<DepartmentDto>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(DepartmentDto createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonSerializer.Serialize(createDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/Departments", content);
                    response.EnsureSuccessStatusCode();
                    TempData["success"] = "Department created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the Department.");
                }
            }
            return View(createDto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Departments/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                response.EnsureSuccessStatusCode();
                var departmentDto = await response.Content.ReadFromJsonAsync<DepartmentDto>();
                return View(departmentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the Department for editing.");
                TempData["error"] = "An error occurred while retrieving the Department for editing.";
                return RedirectToAction(nameof(Index));
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, string name)
        {
            try
            {
                var json = JsonSerializer.Serialize(name);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/Departments/{id}", content);

                response.EnsureSuccessStatusCode();
                TempData["success"] = "Department updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating Department.");

            }
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Departments/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Department deleted successfully!";
                }
                else
                {
                    TempData["error"] = "Department not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while deleting the Department.";
            }

            return RedirectToAction(nameof(Index));
        }
      
    }
}
