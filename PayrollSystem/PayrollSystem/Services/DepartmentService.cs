using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Services;

public class DepartmentService(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await unitOfWork.Departments.GetAllAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await unitOfWork.Departments.GetByIdAsync(id);
    }

    public async Task AddDepartmentAsync(Department department)
    {
        await unitOfWork.Departments.AddAsync(department);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        unitOfWork.Departments.Update(department);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        await unitOfWork.Departments.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }
}