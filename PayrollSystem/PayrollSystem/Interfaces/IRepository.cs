using PayrollSystem.Models.Domain;

namespace PayrollSystem.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    Task DeleteAsync(int id);
}

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> GetWithFiltrationAsync(string? name, bool? hasSalary);
}

public interface ISalaryRepository : IRepository<Salary>
{
    Task<bool> CheckExistenceAsync(JobGrade grade);
}