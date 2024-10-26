using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Repositories;

namespace PayrollSystem.UnitOfWork;

public class UnitOfWork(PayrollContext context) : IUnitOfWork
{
    private IRepository<Employee>? _employees;
    private IRepository<Department>? _departments;
    private IRepository<Salary>? _salaries;
    private IRepository<IncentiveAndDiscount>? _incentives;
    private AttendanceRepository? _attendances;

    public IRepository<Employee> Employees => _employees ??= new Repository<Employee>(context);
    public IRepository<Department> Departments => _departments ??= new Repository<Department>(context);
    public IRepository<Salary> Salaries => _salaries ??= new Repository<Salary>(context);
    public IRepository<IncentiveAndDiscount> Incentives => _incentives ??= new Repository<IncentiveAndDiscount>(context);
    public IAttendanceRepository Attendances => _attendances ??= new AttendanceRepository(context);

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
