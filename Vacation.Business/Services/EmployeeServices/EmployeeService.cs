using Azure.Core;
using System.Data;
using Vacation.Business.DTOs;
using Vacation.Data.Models;
using Vacation.Data.UnitOfWork;
using VacationRequestSystem.Business.Utilities;

namespace Vacation.Business.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto createDto)
        {
            if (createDto == null)
            {
                throw new ArgumentNullException(nameof(createDto), "Employee DTO cannot be null");
            }
            var employee = new Employee
            {
                EmployeeName = createDto.Name,
                Email = createDto.Email,
                Title = createDto.Title,
                DepartmentId = createDto.DepartmentId,
                IsActive = true
            };
            await _unitOfWork.EmployeeRepository.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(e => e.Id == id && e.IsActive);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found");
            }
            employee.IsActive = false;
            _unitOfWork.EmployeeRepository.Update(employee);
            int changes = await _unitOfWork.SaveChangesAsync();
            return changes>0;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAll(e => e.IsActive,includeProperties:"Department");
            return employees.Select(MapToDto);
            //return employees.Select(d => new EmployeeDto { Id = d.Id, Name = d.EmployeeName, Email=d.Email,Title=d.Title,DepartmentId=d.DepartmentId});
        }

        public async Task<EmployeeDto> GetEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(e => e.Id == id && e.IsActive, includeProperties: "Department");
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found");
            }
            return MapToDto(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(int id,EmployeeDto employeeDto)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(e => e.Id == id && e.IsActive);
            if (employee == null)
            {
                throw new ArgumentException($"Employee with ID {id} not found");
            }
            employee.Email = employeeDto.Email;
            employee.Title = employeeDto.Title;
            employee.DepartmentId = employeeDto.DepartmentId;

            _unitOfWork.EmployeeRepository.Update(employee);
            await  _unitOfWork.SaveChangesAsync();
            return MapToDto(employee);
        }

     
        private EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.EmployeeName,
                Email = employee.Email,
                Title = employee.Title,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? string.Empty
            };
        }
    }
}