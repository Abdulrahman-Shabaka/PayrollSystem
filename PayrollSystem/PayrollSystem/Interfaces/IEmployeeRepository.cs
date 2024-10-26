using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> GetWithRelatedDate();
    Task<List<EmployeeReportResponseDto>> GetEmployeesWithAbsencesAndWorkYearsAsync(int month, int year);
}