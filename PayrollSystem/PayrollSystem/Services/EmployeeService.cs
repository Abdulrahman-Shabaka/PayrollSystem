using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Services;

public class EmployeeService(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await unitOfWork.Employees.GetAllAsync();
    }
    public async Task<IEnumerable<Employee>> GetWithRelatedDate()
    {
        return await unitOfWork.Employees.GetWithRelatedDate();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await unitOfWork.Employees.GetByIdAsync(id);
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        await unitOfWork.Employees.AddAsync(employee);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        unitOfWork.Employees.Update(employee);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await unitOfWork.Employees.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }

    public async Task<List<EmployeeReportResponseDto>> GetEmployeesWithAbsencesAndWorkYearsAsync(int month, int year)
    {
       var result = await unitOfWork.Employees.GetEmployeesWithAbsencesAndWorkYearsAsync(month, year);
       return result;
    }
}