using Microsoft.Extensions.Caching.Memory;

using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;

namespace PayrollSystem.Services
{
    public class DepartmentService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        private const string DepartmentsCacheKey = "DepartmentsCacheKey";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        public async Task<IEnumerable<Department>?> GetAllDepartmentsAsync()
        {
            if (memoryCache.TryGetValue(DepartmentsCacheKey, out List<Department>? departments))
                return departments;

            departments = (await unitOfWork.Departments.GetAllAsync()).ToList();
            memoryCache.Set(DepartmentsCacheKey, departments, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheExpiration
            });

            return departments;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            var departments = await GetAllDepartmentsAsync();
            return departments?.FirstOrDefault(d => d.Id == id);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            await unitOfWork.Departments.AddAsync(department);
            await unitOfWork.CompleteAsync();

            if (memoryCache.TryGetValue(DepartmentsCacheKey, out List<Department>? departments))
            {
                departments?.Add(department);
                memoryCache.Set(DepartmentsCacheKey, departments, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheExpiration
                });
            }
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            unitOfWork.Departments.Update(department);
            await unitOfWork.CompleteAsync();

            if (memoryCache.TryGetValue(DepartmentsCacheKey, out List<Department>? departments))
            {
                if (departments != null)
                {
                    var index = departments.FindIndex(d => d.Id == department.Id);
                    if (index >= 0)
                    {
                        departments[index] = department;
                        memoryCache.Set(DepartmentsCacheKey, departments, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = _cacheExpiration
                        });
                    }
                }
            }
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            await unitOfWork.Departments.DeleteAsync(id);
            await unitOfWork.CompleteAsync();

            if (memoryCache.TryGetValue(DepartmentsCacheKey, out List<Department>? departments))
            {
                var departmentToRemove = departments?.FirstOrDefault(d => d.Id == id);
                if (departmentToRemove != null)
                {
                    departments?.Remove(departmentToRemove);
                    memoryCache.Set(DepartmentsCacheKey, departments, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _cacheExpiration
                    });
                }
            }
        }

        public async Task<bool> CheckExistenceAsync(string departmentName)
        {
            var departments = await GetAllDepartmentsAsync();
            return departments != null && departments.Any(s => s.Name == departmentName);
        }
    }
}
