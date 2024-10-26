using PayrollSystem.Models.Domain;

namespace PayrollSystem.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Employee> Employees { get; }
    IRepository<Department> Departments { get; }
    IRepository<Salary> Salaries { get; }
    IRepository<IncentiveAndDiscount> Incentives { get; }
    IAttendanceRepository Attendances { get; }
    Task<int> CompleteAsync();
}