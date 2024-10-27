using Microsoft.EntityFrameworkCore;

using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Repositories;

public class AttendanceRepository(PayrollContext context) : Repository<Attendance>(context), IAttendanceRepository
{
    private readonly PayrollContext _context = context;

    public async Task<IEnumerable<Attendance>> GetFilteredAttendancesAsync(int? employeeId, DateTime? date)
    {
        var query = _context.Attendances
            .AsNoTracking()
            .Include(a => a.Employee)
            .AsQueryable();

        if (employeeId.HasValue)
        {
            query = query.Where(a => a.EmployeeId == employeeId);
        }

        if (date.HasValue)
        {
            query = query.Where(a => a.Date.Date == date.Value.Date);
        }

        return await query
            .OrderByDescending(a => a.Date)
            .ToListAsync();
    }

    public override async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Include(a => a.Employee)
            .FirstOrDefaultAsync();
    }

    public Task<bool> CheckExistenceAsync(int employeeId, DateTime date)
    {
        return _context.Attendances
            .AsNoTracking()
            .AnyAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
    }

    public async Task<IEnumerable<AttendanceReportDto>> GetAttendanceReport(int? month, int? year, int? employeeId)
    {
        return await _context.Attendances
            .AsNoTracking()
            .Where(a =>
                (!employeeId.HasValue || a.EmployeeId == employeeId) &&
                (!year.HasValue || a.Date.Year == year.Value) &&
                (!month.HasValue || a.Date.Month == month.Value) &&
                a.Date >= a.Employee.HireDate)
            .GroupBy(a => new { a.EmployeeId, a.Employee.Name, a.Date.Year, a.Date.Month })
            .Select(g => new AttendanceReportDto
            {
                EmployeeName = g.Key.Name,
                Year = g.Key.Year,
                Month = g.Key.Month,
                DaysAttended = g.Count(a => a.Status == AttendanceStatus.Present),
                DaysAbsent = g.Count(a => a.Status == AttendanceStatus.Absent)
            }).ToListAsync();
    }
}