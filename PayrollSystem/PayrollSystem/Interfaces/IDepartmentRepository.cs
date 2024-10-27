using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IEnumerable<DepartmentWithEmployeeCountDto>> GetDepartmentWithEmployeesCount();
}