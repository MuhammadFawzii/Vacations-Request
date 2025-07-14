using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;
using Vacation.Business.DTOs;
using Vacation.Business.Services;

namespace Vacation.Web.Controllers
{
    public class RequestsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RequestsController> _logger;
        public RequestsController(IHttpClientFactory httpClientFactory, ILogger<RequestsController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/requests");
                response.EnsureSuccessStatusCode();
                var requests = await response.Content.ReadFromJsonAsync<IEnumerable<RequestDto>>() ?? Enumerable.Empty<RequestDto>();
                return View(requests);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving vacation requests.");
                TempData["error"] = "An error occurred while retrieving vacation requests.";
                return View(new List<RequestDto>());
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

        public async Task<IActionResult> Create(CreateRequestDto createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                 
                    var json = JsonSerializer.Serialize(createDto);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/requests", content);
                    response.EnsureSuccessStatusCode();
                    TempData["success"] = "Vacation request created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the vacation request.");
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
                var response = await _httpClient.GetAsync($"api/requests/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                response.EnsureSuccessStatusCode();
                var request = await response.Content.ReadFromJsonAsync<RequestDto>();
                return View(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the vacation request for editing.");
                TempData["error"] = "An error occurred while retrieving the vacation request for editing.";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/requests/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();

                }
                response.EnsureSuccessStatusCode();
                var request = await response.Content.ReadFromJsonAsync<RequestDto>();
                return View(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the vacation request for editing.");
                TempData["error"] = "An error occurred while retrieving the vacation request for editing.";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id,RequestDto requestDTo)
        {
            try
            {
                var json = JsonSerializer.Serialize(requestDTo.RequestStatus);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/requests/{id}/status",content);
      
                response.EnsureSuccessStatusCode();
                TempData["success"] = "Vacation request updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the vacation request.");

            }
            await GetDepartments();
            return View(requestDTo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/requests/{id}");
                response.EnsureSuccessStatusCode();
                var request = await response.Content.ReadFromJsonAsync<RequestDto>();
                return View(request);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Deleting vacation requests.");
                TempData["success"] = "An error occurred while Deleting vacation requests.";
                return View(new List<RequestDto>());
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/requests/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Vacation request deleted successfully!";
                }
                else
                {
                    TempData["error"] = "Vacation request not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while deleting the vacation request.";
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
    }
}
