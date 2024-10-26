using System.ComponentModel.DataAnnotations;

using PayrollSystem.Models.Domain;

namespace PayrollSystem.Models.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public DateTime DateOfBirth { get; set; }

    [Required]
    public required string Address { get; set; }

    [Required]
    [Phone]
    public required string Phone { get; set; }

    [Required]
    public JobGrade JobGrade { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public int DepartmentId { get; set; }

    public DateTime HireDate { get; set; }

    public string? DepartmentName { get; set; }
    public AttendanceDto? Attendance { get; set; }
}