using Microsoft.Extensions.Caching.Memory;

using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Services
{
    public class SalaryService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        private const string SalariesCacheKey = "SalariesCacheKey";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        public async Task<IEnumerable<Salary>> GetAllSalariesAsync()
        {
            if (memoryCache.TryGetValue(SalariesCacheKey, out List<Salary>? salaries))
                return salaries ?? [];

            salaries = (await unitOfWork.Salaries.GetAllAsync()).ToList();
            memoryCache.Set(SalariesCacheKey, salaries, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheExpiration
            });

            return salaries;
        }

        public async Task<Salary?> GetByIdAsync(int id)
        {
            var salaries = await GetAllSalariesAsync();
            return salaries?.FirstOrDefault(s => s.Id == id);
        }

        public async Task AddSalaryAsync(Salary salary)
        {
            await unitOfWork.Salaries.AddAsync(salary);
            await unitOfWork.CompleteAsync();

            if (memoryCache.TryGetValue(SalariesCacheKey, out List<Salary>? salaries))
            {
                salaries?.Add(salary);
                memoryCache.Set(SalariesCacheKey, salaries, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheExpiration
                });
            }
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            unitOfWork.Salaries.Update(salary);
            await unitOfWork.CompleteAsync();

            if (memoryCache.TryGetValue(SalariesCacheKey, out List<Salary>? salaries))
            {
                if (salaries != null)
                {
                    var index = salaries.FindIndex(s => s.Id == salary.Id);
                    if (index >= 0)
                    {
                        salaries[index] = salary;
                        memoryCache.Set(SalariesCacheKey, salaries, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = _cacheExpiration
                        });
                    }
                }
            }
        }

        public async Task DeleteSalaryAsync(int id)
        {
            await unitOfWork.Salaries.DeleteAsync(id);
            await unitOfWork.CompleteAsync();

            if (memoryCache.TryGetValue(SalariesCacheKey, out List<Salary>? salaries))
            {
                var salaryToRemove = salaries?.FirstOrDefault(s => s.Id == id);
                if (salaryToRemove != null)
                {
                    salaries?.Remove(salaryToRemove);
                    memoryCache.Set(SalariesCacheKey, salaries, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _cacheExpiration
                    });
                }
            }
        }

        public async Task<bool> CheckExistenceAsync(JobGrade salaryJobGrade)
        {
            var salaries = await GetAllSalariesAsync();
            return salaries.Any(s => s.JobGrade == salaryJobGrade);
        }
    }
}
