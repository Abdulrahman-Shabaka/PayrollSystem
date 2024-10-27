using Microsoft.EntityFrameworkCore;

using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Repositories;

public class EmployeeRepository(PayrollContext context) : Repository<Employee>(context), IEmployeeRepository
{
    private readonly PayrollContext _context = context;

    public async Task<IEnumerable<Employee>> GetWithRelatedDate()
    {

        return await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .Include(e => e.Attendances.Where(a => a.Date.Date == DateTime.Now.Date))
            .ToListAsync();
    }

    public async Task<List<EmployeeReportResponseDto>> GetEmployeesWithAbsencesAndWorkYearsAsync(int month, int year)
    {
        var specifiedDate = new DateTime(year, month, 1);

        var results = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .Where(e => e.HireDate <= specifiedDate)
            .Select(e => new
            {
                Employee = e,
                AbsentDays = _context.Attendances
                    .Count(a => a.EmployeeId == e.Id && a.Date.Month == month && a.Date.Year == year && a.Status == AttendanceStatus.Absent)
            })
            .Where(e => e.AbsentDays > 0 ||
                        !_context.Attendances.Any(a => a.EmployeeId == e.Employee.Id && a.Date.Month == month && a.Date.Year == year))
            .Select(e => new EmployeeReportResponseDto
            {
                EmployeeId = e.Employee.Id,
                EmployeeName = e.Employee.Name,
                AbsentDays = e.AbsentDays,
                TotalWorkYears = Math.Floor((specifiedDate - e.Employee.HireDate).TotalDays / 365.25),
                JobGrade = e.Employee.JobGrade,
                DepartmentId = e.Employee.Department.Id,
                DepartmentName = e.Employee.Department.Name
            })
            .ToListAsync();

        return results;
    }
}