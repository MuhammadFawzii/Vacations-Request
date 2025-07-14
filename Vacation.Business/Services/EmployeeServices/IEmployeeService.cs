using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation.Business.DTOs;

namespace Vacation.Business.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto createDto);
        Task<EmployeeDto> GetEmployeeAsync(int id);
        Task<EmployeeDto> UpdateEmployeeAsync(int id, EmployeeDto employeeDto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
