using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Models.Dtos;

public class DepartmentWithEmployeeCountDto
{
    public int Id { get; set; }
    [Required] public required string Name { get; set; }
    public string? Description { get; set; }
    public int EmployeeCount { get; set; }
}