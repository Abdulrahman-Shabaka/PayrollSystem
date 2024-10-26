using Microsoft.Extensions.Caching.Memory;

using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Services;

public class IncentiveService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
{
    private const string IncentivesCacheKey = "IncentivesCacheKey";

    public async Task<IEnumerable<IncentiveAndDiscount>?> GetAllIncentivesAsync()
    {
        if (memoryCache.TryGetValue(IncentivesCacheKey, out List<IncentiveAndDiscount>? incentives)) return incentives;
        incentives = (await unitOfWork.Incentives.GetAllAsync()).ToList();
        memoryCache.Set(IncentivesCacheKey, incentives);

        return incentives;
    }

    public async Task<IncentiveAndDiscount?> GetByIdAsync(int id)
    {
        var incentives = await GetAllIncentivesAsync();
        return incentives?.FirstOrDefault(i => i.Id == id);
    }

    public async Task AddIncentiveAsync(IncentiveAndDiscount incentiveAndDiscount)
    {
        await unitOfWork.Incentives.AddAsync(incentiveAndDiscount);
        await unitOfWork.CompleteAsync();

        if (memoryCache.TryGetValue(IncentivesCacheKey, out List<IncentiveAndDiscount>? incentives))
        {
            incentives?.Add(incentiveAndDiscount);
            memoryCache.Set(IncentivesCacheKey, incentives);
        }
    }

    public async Task UpdateIncentiveAsync(IncentiveAndDiscount incentiveAndDiscount)
    {
        unitOfWork.Incentives.Update(incentiveAndDiscount);
        await unitOfWork.CompleteAsync();

        if (memoryCache.TryGetValue(IncentivesCacheKey, out List<IncentiveAndDiscount>? incentives))
        {
            if (incentives != null)
            {
                var index = incentives.FindIndex(i => i.Id == incentiveAndDiscount.Id);
                if (index >= 0)
                {
                    incentives[index] = incentiveAndDiscount;
                    memoryCache.Set(IncentivesCacheKey, incentives);
                }
            }
        }
    }

    public async Task DeleteIncentiveAsync(int id)
    {
        await unitOfWork.Incentives.DeleteAsync(id);
        await unitOfWork.CompleteAsync();

        if (memoryCache.TryGetValue(IncentivesCacheKey, out List<IncentiveAndDiscount>? incentives))
        {
            var incentiveToRemove = incentives?.FirstOrDefault(i => i.Id == id);
            if (incentiveToRemove != null)
            {
                incentives?.Remove(incentiveToRemove);
                memoryCache.Set(IncentivesCacheKey, incentives);
            }
        }
    }

    public async Task<bool> CheckExistenceAsync(IncentiveAndDiscountType type)
    {
        var incentives = await GetAllIncentivesAsync();
        return incentives != null && incentives.Any(i => i.Type == type);
    }
}