using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Services;

public class SalaryService(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<Salary>> GetAllSalariesAsync()
    {
        return await unitOfWork.Salaries.GetAllAsync();
    }

    public async Task<Salary?> GetByIdAsync(int id)
    {
        return await unitOfWork.Salaries.GetByIdAsync(id);
    }

    public async Task AddSalaryAsync(Salary salary)
    {
        await unitOfWork.Salaries.AddAsync(salary);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateSalaryAsync(Salary salary)
    {
        unitOfWork.Salaries.Update(salary);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteSalaryAsync(int id)
    {
        await unitOfWork.Salaries.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }

    public async Task<bool> CheckExistenceAsync(JobGrade salaryJobGrade)
    {
        return await unitOfWork.Salaries.CheckExistenceAsync(salaryJobGrade);
    }
}