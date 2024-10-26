using PayrollSystem.Models.Domain;

namespace PayrollSystem.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    IRepository<Department> Departments { get; }
    IRepository<Salary> Salaries { get; }
    IIncentiveAndDiscountRepository Incentives { get; }
    IAttendanceRepository Attendances { get; }
    Task<int> CompleteAsync();
}