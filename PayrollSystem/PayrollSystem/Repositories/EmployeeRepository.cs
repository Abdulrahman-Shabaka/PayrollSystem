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
        // Create a date representing the first day of the specified month and year
        var specifiedDate = new DateTime(year, month, 1);

        var results = await _context.Employees
            .Include(e => e.Department) // Include the Department information
            .GroupJoin(
                _context.Attendances.Where(a => a.Date.Month == month && a.Date.Year == year), // Filter attendances for the specific month and year
                employee => employee.Id,
                attendance => attendance.EmployeeId,
                (employee, attendances) => new
                {
                    Employee = employee,
                    Attendances = attendances
                })
            .SelectMany(
                x => x.Attendances.DefaultIfEmpty(), // Left join to include employees without attendance records
                (x, attendance) => new
                {
                    x.Employee,
                    Attendance = attendance
                })
            // Check for absence using the AttendanceStatus enum
            .Where(x => x.Attendance == null || x.Attendance.Status == AttendanceStatus.Absent)
            .GroupBy(
                x => new
                {
                    x.Employee.Id,
                    x.Employee.Name,
                    x.Employee.HireDate,
                    x.Employee.JobGrade, // Include JobGrade in grouping
                    DepartmentId = x.Employee.Department.Id, // Use unique names
                    DepartmentName = x.Employee.Department.Name // Use unique names
                })
            .Select(g => new EmployeeReportResponseDto()
            {
                EmployeeId = g.Key.Id,
                EmployeeName = g.Key.Name,
                AbsentDays = g.Count(x => x.Attendance == null || x.Attendance.Status == AttendanceStatus.Absent), // Count absent days
                                                                                                                   // Calculate total work years using the specified date
                TotalWorkYears = Math.Floor((specifiedDate - g.Key.HireDate).TotalDays / 365.25), // Calculate total work years
                JobGrade = g.Key.JobGrade, // Include JobGrade in the result
                DepartmentId = g.Key.DepartmentId, // Use the unique name for Department ID
                DepartmentName = g.Key.DepartmentName // Use the unique name for Department Name
            })
            .ToListAsync(); // Execute the query asynchronously

        return results;
    }
}