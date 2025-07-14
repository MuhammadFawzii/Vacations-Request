using Vacation.Business.DTOs;
using Vacation.Data.Models;
using Vacation.Data.UnitOfWork;
using VacationRequestSystem.Business.Utilities;

namespace Vacation.Business.Services.RequestServices
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RequestDto>> GetAllRequestsAsync()
        {
            var requests = await _unitOfWork.RequestRepository.GetAll(
                r => r.IsActive == true,
                includeProperties: "Employee,Employee.Department");

            return requests.Select(MapToDto);
        }
        public async Task<RequestDto> CreateRequestAsync(CreateRequestDto createDto)
        {
            if (createDto == null)
            {
                throw new ArgumentNullException(nameof(createDto), "CreateRequestDto cannot be null");
            }
            if (createDto.VacationDateFrom < DateTime.Today)
            {
                throw new ArgumentException("Vacation start date cannot be in the past");
            }
            if (createDto.VacationDateTo < createDto.VacationDateFrom)
            {
                throw new ArgumentException("Vacation end date cannot be before the start date");
            }
            var returningDate = CalculateReturningDate(createDto.VacationDateTo);
            // Calculate total days requested
            var totalDays = CalculateTotalDays(createDto.VacationDateFrom, createDto.VacationDateTo);
            // Create or retrieve the employee
            var employee = await CreateEmployeeAsync(createDto);
            // Create the vacation request
            var request = new Request
            {
                EmployeeId = employee.Id,
                VacationDateFrom = DateOnly.FromDateTime(createDto.VacationDateFrom),
                VacationDateTo = DateOnly.FromDateTime(createDto.VacationDateTo),
                ReturningDate = DateOnly.FromDateTime(returningDate),
                SubmissionDate = DateTime.Now,
                TotalDaysRequested = totalDays,
                Notes = createDto.Notes,
                RequestStatus = "Pending",
                IsActive = true
            };
            // Add the request to the repository
            await _unitOfWork.RequestRepository.AddAsync(request);
            await _unitOfWork.SaveChangesAsync();
            var requestDetails = await _unitOfWork.RequestRepository.GetAsync(r => r.Id == request.Id, includeProperties: "Employee,Employee.Department");
            return MapToDto(requestDetails);

        }

        public async Task<RequestDto?> GetRequestAsync(int id)
        {
            var request = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, includeProperties: "Employee,Employee.Department");
            return request != null ? MapToDto(request) : null;
        }

        public async Task<RequestDto> UpdateRequestAsync(int id, string status)
        {
            Request? request = null;
            if (IsVaildStatus(status))
            {
                request = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id, includeProperties: "Employee,Employee.Department");
                if (request != null)
                {
                    request.RequestStatus = status;
                    _unitOfWork.RequestRepository.Update(request);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Vacation request not found");
                }
            }
            return MapToDto(request!);
        }
        public async Task<bool> DeleteRequestAsync(int id)
        {
            var request = await _unitOfWork.RequestRepository.GetAsync(x => x.Id == id);
            if (request == null)
            {
                return false;
            }
            request.IsActive = false;
            _unitOfWork.RequestRepository.Update(request);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }
        public bool IsVaildStatus(string status)
        {
            return new[] { SD.Approved, SD.Rejected, SD.Cancelled, SD.Pending }.Contains(status);

        }
        private RequestDto MapToDto(Request request)
        {
            return new RequestDto
            {
                Id = request.Id,
                EmployeeName = request.Employee.EmployeeName,
                Title = request.Employee.Title,
                DepartmentId = request.Employee.DepartmentId,
                DepartmentName = request.Employee.Department.Name,
                SubmissionDate = request.SubmissionDate,
                VacationDateFrom = request.VacationDateFrom.ToDateTime(new TimeOnly(0, 0)),
                VacationDateTo = request.VacationDateTo.ToDateTime(new TimeOnly(0, 0)),
                ReturningDate = request.ReturningDate.ToDateTime(new TimeOnly(0, 0)),
                TotalDaysRequested = request.TotalDaysRequested,
                Notes = request.Notes,
                RequestStatus = request.RequestStatus
            };
        }

        private DateTime CalculateReturningDate(DateTime vacationEndDate)
        {
            var returningDate = vacationEndDate.AddDays(1);
            while (returningDate.DayOfWeek == DayOfWeek.Friday || returningDate.DayOfWeek == DayOfWeek.Saturday)
            {
                returningDate = returningDate.AddDays(1);
            }
            return returningDate;

        }
        private int CalculateTotalDays(DateTime startDate, DateTime endDate)
        {
            var totalDays = 0;
            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDays++;
                }
                currentDate = currentDate.AddDays(1);
            }
            return totalDays;
        }
        private async Task<Employee> CreateEmployeeAsync(CreateRequestDto createDto)
        {
            Employee entityEmployee = await _unitOfWork.EmployeeRepository.GetAsync(e => e.EmployeeName == createDto.EmployeeName && e.DepartmentId == createDto.DepartmentId && e.IsActive);

            if (entityEmployee == null)
            {
                entityEmployee = new Employee
                {
                    EmployeeName = createDto.EmployeeName,
                    Title = createDto.Title,
                    DepartmentId = createDto.DepartmentId,
                    IsActive = true
                };
                await _unitOfWork.EmployeeRepository.AddAsync(entityEmployee);
                await _unitOfWork.SaveChangesAsync();
            }
            return entityEmployee;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll(
                d => d.IsActive);
            return departments.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name,IsActive=d.IsActive });

        }

    }
}