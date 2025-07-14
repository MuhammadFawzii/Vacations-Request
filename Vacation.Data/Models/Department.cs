﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vacation.Data.Models;
[Table("Departments")]
public partial class Department
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }=true;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
