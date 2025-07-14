using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation.Business.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; } = default;
        public string Name { get; set; } = string.Empty;
        public string Title{ get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public int DepartmentId { get; set; } = default;
        public string DepartmentName { get; set; } = string.Empty;
    }
}
