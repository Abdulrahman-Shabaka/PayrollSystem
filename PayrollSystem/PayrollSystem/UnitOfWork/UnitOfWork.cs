using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Repositories;

namespace PayrollSystem.UnitOfWork;

public class UnitOfWork(PayrollContext context) : IUnitOfWork
{
    private IEmployeeRepository? _employees;
    private IRepository<Department>? _departments;
    private IRepository<Salary>? _salaries;
    private IIncentiveAndDiscountRepository? _incentives;
    private IAttendanceRepository? _attendances;

    public IEmployeeRepository Employees => _employees ??= new EmployeeRepository(context);
    public IRepository<Department> Departments => _departments ??= new Repository<Department>(context);
    public IRepository<Salary> Salaries => _salaries ??= new Repository<Salary>(context);
    public IIncentiveAndDiscountRepository Incentives => _incentives ??= new IncentiveAndDiscountRepository(context);
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
