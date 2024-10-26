using System.ComponentModel.DataAnnotations;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Models.Dtos;

public class AttendanceDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    [Required] public DateTime Date { get; set; }
    [DataType(DataType.Time)] public DateTime? LoginTime { get; set; }
    [DataType(DataType.Time)] public DateTime? LogoutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Reason { get; set; }
    public string? EmployeeName { get; set; }

}