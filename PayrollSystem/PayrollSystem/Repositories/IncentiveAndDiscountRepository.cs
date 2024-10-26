using Microsoft.EntityFrameworkCore;
using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Repositories;

public class IncentiveAndDiscountRepository(PayrollContext context) : Repository<IncentiveAndDiscount>(context), IIncentiveAndDiscountRepository
{
    public override async Task<IEnumerable<IncentiveAndDiscount>> GetAllAsync()
    {
        return await context.Incentives
            .AsNoTracking()
            .Include(i => i.Department)
            .ToListAsync();
    }
}