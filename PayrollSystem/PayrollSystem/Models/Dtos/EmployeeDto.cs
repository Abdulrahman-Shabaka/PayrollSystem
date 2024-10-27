using System.ComponentModel.DataAnnotations;

using PayrollSystem.Models.Domain;

namespace PayrollSystem.Models.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }
    [Required] public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    [Required] public required string Address { get; set; }
    [Required] [Phone] public required string Phone { get; set; }
    [Required] public JobGrade JobGrade { get; set; }
    [EmailAddress] public string? Email { get; set; }
    public int DepartmentId { get; set; }
    public DateTime HireDate { get; set; }
    public string? DepartmentName { get; set; }
    public AttendanceDto? Attendance { get; set; }
}

public class EmployeeReportResponseDto
{
    public int EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public JobGrade JobGrade { get; set; }
    public int AbsentDays { get; set; }
    public double TotalWorkYears { get; set; }
    public int DepartmentId { get; set; }
    public required string DepartmentName { get; set; }
}

public class EmployeeReportViewDto : EmployeeReportResponseDto
{
    public decimal BaseSalary { get; set; }
    public decimal Incentive { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalSalary { get; set; }
}