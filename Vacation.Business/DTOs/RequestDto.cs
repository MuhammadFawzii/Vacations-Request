
using System.ComponentModel.DataAnnotations;

namespace Vacation.Business.DTOs;
public class RequestDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Employee name is required")]
    [StringLength(255, ErrorMessage = "Employee name cannot exceed 255 characters")]
    public string EmployeeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Title is required")]
    [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public DateTime SubmissionDate { get; set; }

    [Required(ErrorMessage = "Vacation start date is required")]
    public DateTime VacationDateFrom { get; set; }

    [Required(ErrorMessage = "Vacation end date is required")]
    public DateTime VacationDateTo { get; set; }

    public DateTime ReturningDate { get; set; }

    public int TotalDaysRequested { get; set; }

    public string? Notes { get; set; }
    public string RequestStatus { get; set; } = "Pending";
}
