using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vacation.Data.Models;
[Table("Requests")]
public partial class Request
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public DateTime SubmissionDate { get; set; }
    [Required]
    [Column(TypeName = "date")]
    public DateOnly VacationDateFrom { get; set; }
    [Required]
    [Column(TypeName = "date")]
    public DateOnly VacationDateTo { get; set; }
    [Required]
    [Column(TypeName = "date")]
    public DateOnly ReturningDate { get; set; }
    [Required]
    public int TotalDaysRequested { get; set; }
    public string? Notes { get; set; }
    [Required]
    [StringLength(50)]
    public string RequestStatus { get; set; } = "Pending";
    public bool IsActive { get; set; }=true;
    [ForeignKey("EmployeeId")]
    public virtual Employee Employee { get; set; } = null!;
}
