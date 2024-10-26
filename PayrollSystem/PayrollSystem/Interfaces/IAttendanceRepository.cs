using PayrollSystem.Models.Domain;

namespace PayrollSystem.Interfaces;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<bool> CheckExistenceAsync(int employeeId, DateTime date);
}