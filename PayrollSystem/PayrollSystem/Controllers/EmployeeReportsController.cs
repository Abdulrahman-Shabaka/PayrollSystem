using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

using PayrollSystem.Services;

namespace PayrollSystem.Controllers
{
    public class EmployeeReportsController(EmployeeService employeeService, SalaryService salaryService, IncentiveAndDiscountService incentiveAndDiscountService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index(int? month, int? year)
        {

            if (!month.HasValue || !year.HasValue)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            var employeeReportResponseDtos = await employeeService.GetEmployeesWithAbsencesAndWorkYearsAsync((int)month, (int)year);
            var salaries = await salaryService.GetAllSalariesAsync();
            var incentiveAndDiscounts = await incentiveAndDiscountService.GetAllIncentivesAsync();

            var employeeReportViewDtos = new List<EmployeeReportViewDto>();

            foreach (var employee in employeeReportResponseDtos)
            {
                var employeeReportViewDto = mapper.Map<EmployeeReportViewDto>(employee);

                employeeReportViewDto.BaseSalary =
                    salaries.First(s => s.JobGrade == employeeReportViewDto.JobGrade).BaseSalary;

                if (employeeReportViewDto.AbsentDays < 2)
                    employeeReportViewDto.Incentive += incentiveAndDiscounts.FirstOrDefault(i => i.Type == IncentiveAndDiscountType.AttendanceIncentive).Percentage;

                employeeReportViewDto.Incentive += incentiveAndDiscounts.FirstOrDefault(i => i.DepartmentId == employeeReportViewDto.DepartmentId).Percentage;

                var numberOfYearsIncentives = incentiveAndDiscounts
                    .Where(i => i.Type == IncentiveAndDiscountType.NumberOfYearsIncentive).ToList();

                foreach (var numOfYearsIncentive in numberOfYearsIncentives)
                {
                    if (numOfYearsIncentive.MaximumYears >= employeeReportViewDto.TotalWorkYears &&
                        numOfYearsIncentive.MaximumYears <= employeeReportViewDto.TotalWorkYears)
                        employeeReportViewDto.Incentive += numOfYearsIncentive.Percentage;
                }

                if (employeeReportViewDto.AbsentDays is > 2 and <= 5)
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanTwoDay)
                        .Percentage;

                else if (employeeReportViewDto.AbsentDays is >5 and <= 10)
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanFiveDay)
                        .Percentage;

                else if (employeeReportViewDto.AbsentDays is > 10 and <= 15)
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanTenDay)
                        .Percentage;

                else if (employeeReportViewDto.AbsentDays is > 15 )
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanFifteenDay)
                        .Percentage;

                employeeReportViewDto.TotalSalary = employeeReportViewDto.BaseSalary + (employeeReportViewDto.BaseSalary * employeeReportViewDto.Incentive)/100
                                                    - (employeeReportViewDto.BaseSalary * employeeReportViewDto.Discount)/100;

                employeeReportViewDtos.Add(employeeReportViewDto);
            }

            ViewBag.Month = month.Value;
            ViewBag.Year = year.Value;

            return View(employeeReportViewDtos);
        }
    }
}
