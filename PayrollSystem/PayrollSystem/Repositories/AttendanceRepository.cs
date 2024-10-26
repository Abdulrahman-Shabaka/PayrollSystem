using Microsoft.EntityFrameworkCore;
using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Repositories;

public class AttendanceRepository(PayrollContext context) : Repository<Attendance>(context), IAttendanceRepository
{
    private readonly PayrollContext _context = context;

    public override async Task<IEnumerable<Attendance>> GetAllAsync()
    {
        return await _context.Attendances
            .AsNoTracking()
            .OrderByDescending(a => a.Date)
            .Include(a => a.Employee)
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
}