using Microsoft.EntityFrameworkCore;

using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Repositories;

public class Repository<T>(PayrollContext context) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null) _dbSet.Remove(entity);
    }
}

public class SalaryRepository(PayrollContext context) : Repository<Salary>(context), ISalaryRepository
{
    private readonly PayrollContext _context = context;

    public async Task<bool> CheckExistenceAsync(JobGrade grade)
    {
        return await _context.Set<Salary>().AnyAsync(s => s.JobGrade == grade);
    }
}

//public class EmployeeRepository(PayrollContext context) : Repository<Employee>(context), IEmployeeRepository
//{
//    public async Task<IEnumerable<Employee>> GetWithFiltrationAsync(string? name, bool? hasSalary)
//    {
//        var query = context.Set<Employee>().Include(e => e.Salary).AsQueryable();

//        if (!string.IsNullOrEmpty(name))
//        {
//            query = query.Where(e => e.Name.Contains(name));
//        }

//        if (!hasSalary.HasValue) return await query.ToListAsync();
//        {
//            query = hasSalary.Value ? query.Where(e => e.Salary != null) : query.Where(e => e.Salary == null);
//        }

//        return await query.ToListAsync();
//    }
//}