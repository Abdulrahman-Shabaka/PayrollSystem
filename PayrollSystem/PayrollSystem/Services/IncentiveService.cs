using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Services;

public class IncentiveService(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<IncentiveAndDiscount>> GetAllIncentivesAsync()
    {
        return await unitOfWork.Incentives.GetAllAsync();
    }

    public async Task<IncentiveAndDiscount?> GetByIdAsync(int id)
    {
        return await unitOfWork.Incentives.GetByIdAsync(id);
    }

    public async Task AddIncentiveAsync(IncentiveAndDiscount incentiveAndDiscount)
    {
        await unitOfWork.Incentives.AddAsync(incentiveAndDiscount);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateIncentiveAsync(IncentiveAndDiscount incentiveAndDiscount)
    {
        unitOfWork.Incentives.Update(incentiveAndDiscount);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteIncentiveAsync(int id)
    {
        await unitOfWork.Incentives.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }
}