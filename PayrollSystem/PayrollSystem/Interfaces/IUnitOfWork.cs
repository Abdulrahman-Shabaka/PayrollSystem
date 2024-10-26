using PayrollSystem.Models.Domain;

namespace PayrollSystem.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Employee> Employees { get; }
    IRepository<Department> Departments { get; }
    ISalaryRepository Salaries { get; }
    IRepository<IncentiveAndDiscount> Incentives { get; }
    IRepository<Attendance> Attendances { get; }
    Task<int> CompleteAsync();
}