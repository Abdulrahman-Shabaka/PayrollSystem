using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Repositories;

namespace PayrollSystem.UnitOfWork;

public class UnitOfWork(PayrollContext context) : IUnitOfWork
{
    private IRepository<Employee>? _employees;
    private IRepository<Department>? _departments;
    private ISalaryRepository? _salaries;
    private IRepository<IncentiveAndDiscount>? _incentives;
    private IRepository<Attendance>? _attendances;

    public IRepository<Employee> Employees => _employees ??= new Repository<Employee>(context);
    public IRepository<Department> Departments => _departments ??= new Repository<Department>(context);
    public ISalaryRepository Salaries => _salaries ??= new SalaryRepository(context);
    public IRepository<IncentiveAndDiscount> Incentives => _incentives ??= new Repository<IncentiveAndDiscount>(context);
    public IRepository<Attendance> Attendances => _attendances ??= new Repository<Attendance>(context);

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
