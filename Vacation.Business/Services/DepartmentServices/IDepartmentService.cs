using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation.Business.DTOs;

namespace Vacation.Business.Services.DepartmentServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto createDto);
        Task<DepartmentDto> GetDepartmenAsync(int id);
        Task<DepartmentDto> UpdateDepartmentAsync(int id, string name);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
