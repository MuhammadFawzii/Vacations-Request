using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vacation.Data.Models;
[Table("Employees")]
public partial class Employee
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string EmployeeName { get; set; } = null!;

    public string Title { get; set; } = null!;
    [Required]
    public int DepartmentId { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }=true;
    [ForeignKey("DepartmentId")]
    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
