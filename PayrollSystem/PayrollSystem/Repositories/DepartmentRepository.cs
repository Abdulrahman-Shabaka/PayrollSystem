using Microsoft.EntityFrameworkCore;

using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Repositories;

public class DepartmentRepository(PayrollContext context) : Repository<Department>(context), IDepartmentRepository
{
    private readonly PayrollContext _context = context;

    public async Task<IEnumerable<DepartmentWithEmployeeCountDto>> GetDepartmentWithEmployeesCount()
    {
        return await _context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentWithEmployeeCountDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                EmployeeCount = d.Employees.Count()
            })
            .ToListAsync();
    }
}