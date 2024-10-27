using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Interfaces;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetFilteredAttendancesAsync(int? employeeId, DateTime? date);
    Task<bool> CheckExistenceAsync(int employeeId, DateTime date);
    Task<IEnumerable<AttendanceReportDto>> GetAttendanceReport(int? month, int? year, int? employeeId);
}