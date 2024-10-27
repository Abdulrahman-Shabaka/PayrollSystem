using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Models.Domain;

public class Attendance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    [Required] public DateTime Date { get; set; }
    [DataType(DataType.Time)] public DateTime? LoginTime { get; set; }
    [DataType(DataType.Time)] public DateTime? LogoutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Reason { get; set; }

    public virtual Employee Employee { get; set; }
}