using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Models.ViewModels;

public class EmployeeReportViewModel
{
    public IEnumerable<EmployeeReportViewDto> EmployeesReports { get; set; } = new List<EmployeeReportViewDto>();
    public int SelectedMonth { get; set; }
    public int SelectedYear { get; set; }
}