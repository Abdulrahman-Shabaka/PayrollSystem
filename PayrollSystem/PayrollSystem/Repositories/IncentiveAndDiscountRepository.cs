using Microsoft.EntityFrameworkCore;

using PayrollSystem.Data;
using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Repositories;

public class IncentiveAndDiscountRepository(PayrollContext context) : Repository<IncentiveAndDiscount>(context), IIncentiveAndDiscountRepository
{
    private readonly PayrollContext _context = context;

    public override async Task<IEnumerable<IncentiveAndDiscount>> GetAllAsync()
    {
        return await _context.Incentives
            .AsNoTracking()
            .Include(i => i.Department)
            .ToListAsync();
    }
}