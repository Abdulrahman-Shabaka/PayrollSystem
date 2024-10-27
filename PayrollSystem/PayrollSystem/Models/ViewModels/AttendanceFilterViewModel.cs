using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Models.ViewModels;

public class AttendanceFilterViewModel
{
    public IEnumerable<Attendance> Attendances { get; set; } = new List<Attendance>();
    public int? EmployeeId { get; set; }
    public DateTime? SelectedDate { get; set; }
}
public class AttendanceReportViewModel
{
    public IEnumerable<AttendanceReportDto> AttendanceReports { get; set; } = new List<AttendanceReportDto>();
    public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    public int? SelectedMonth { get; set; }
    public int? SelectedYear { get; set; }
    public int? SelectedEmployeeId { get; set; }
}