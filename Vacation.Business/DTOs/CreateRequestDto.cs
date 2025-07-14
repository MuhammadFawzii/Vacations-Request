using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation.Business.DTOs
{
    public class CreateRequestDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(255, ErrorMessage = "Employee name cannot exceed 255 characters")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Vacation start date is required")]
        public DateTime VacationDateFrom { get; set; }

        [Required(ErrorMessage = "Vacation end date is required")]
        public DateTime VacationDateTo { get; set; }

        public string? Notes { get; set; }
    }
}
