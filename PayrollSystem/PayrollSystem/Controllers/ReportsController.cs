using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;
using PayrollSystem.Models.ViewModels;
using PayrollSystem.Services;

namespace PayrollSystem.Controllers
{
    public class ReportsController(EmployeeService employeeService, SalaryService salaryService,
        IncentiveAndDiscountService incentiveAndDiscountService,DepartmentService departmentService,AttendanceService attendanceService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Employee(EmployeeReportViewModel model)
        {

            model.SelectedMonth = model.SelectedMonth > 0 ? model.SelectedMonth : DateTime.Now.Month;
            model.SelectedYear = model.SelectedYear > 0 ? model.SelectedYear : DateTime.Now.Year;

            var employeeReportResponseDtos = await employeeService.GetEmployeesWithAbsencesAndWorkYearsAsync(model.SelectedMonth, model.SelectedYear);
            var salaries = await salaryService.GetAllSalariesAsync();
            var incentiveAndDiscounts = await incentiveAndDiscountService.GetAllIncentivesAsync();

            var employeeReportViewDtos = employeeReportResponseDtos.Select(employee =>
            {
                var employeeReportViewDto = mapper.Map<EmployeeReportViewDto>(employee);
                employeeReportViewDto.BaseSalary = salaries.FirstOrDefault(s => s.JobGrade == employeeReportViewDto.JobGrade)?.BaseSalary ?? 0;

                CalculateIncentives(employeeReportViewDto, (List<IncentiveAndDiscount>)incentiveAndDiscounts);
                CalculateDiscounts(employeeReportViewDto, (List<IncentiveAndDiscount>)incentiveAndDiscounts);
                CalculateTotalSalary(employeeReportViewDto);

                return employeeReportViewDto;
            }).ToList();

            var viewModel = new EmployeeReportViewModel
            {
                EmployeesReports = employeeReportViewDtos,
                SelectedMonth = model.SelectedMonth,
                SelectedYear = model.SelectedYear,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Department()
        {
            var departments = await departmentService.GetAllDepartmentsWithEmployeeCountAsync();
            return View(departments);
        }

        public async Task<IActionResult> IncentiveAndDiscounts()
        {
            ViewBag.IsReport = true;

            var discounts = await incentiveAndDiscountService.GetAllIncentivesAsync();
            return View("~/Views/IncentiveAndDiscount/index.cshtml", discounts);
        }

        public async Task<IActionResult> Attendance(AttendanceReportViewModel model)
        {
            model.SelectedMonth ??= DateTime.Now.Month;
            model.SelectedYear ??= DateTime.Now.Year;

            var attendanceReport = await attendanceService.GetAttendanceReport(model.SelectedMonth, model.SelectedYear, model.SelectedEmployeeId);

            var viewModel = new AttendanceReportViewModel
            {
                AttendanceReports = attendanceReport,
                SelectedEmployeeId = model.SelectedEmployeeId,
                SelectedMonth = model.SelectedMonth,
                SelectedYear = model.SelectedYear
            };

            ViewBag.Employees = await employeeService.GetAllEmployeesAsync();
            if (model.SelectedEmployeeId != null) ViewBag.SelectedEmployeeId = model.SelectedEmployeeId;

            return View(viewModel);
        }

        private static void CalculateIncentives(EmployeeReportViewDto employeeReportViewDto, List<IncentiveAndDiscount> incentiveAndDiscounts)
        {
            if (employeeReportViewDto.AbsentDays < 2)
                employeeReportViewDto.Incentive += incentiveAndDiscounts
                    .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.AttendanceIncentive)?.Percentage ?? 0;

            employeeReportViewDto.Incentive += incentiveAndDiscounts
                .FirstOrDefault(i => i.DepartmentId == employeeReportViewDto.DepartmentId)?.Percentage ?? 0;

            var numberOfYearsIncentives = incentiveAndDiscounts
                .Where(i => i.Type == IncentiveAndDiscountType.NumberOfYearsIncentive);

            foreach (var numOfYearsIncentive in numberOfYearsIncentives)
            {
                if (numOfYearsIncentive.MaximumYears >= employeeReportViewDto.TotalWorkYears)
                    employeeReportViewDto.Incentive += numOfYearsIncentive.Percentage;
            }
        }

        private static void CalculateDiscounts(EmployeeReportViewDto employeeReportViewDto, List<IncentiveAndDiscount> incentiveAndDiscounts)
        {
            switch (employeeReportViewDto.AbsentDays)
            {
                case > 2 and <= 5:
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanTwoDay)?.Percentage ?? 0;
                    break;
                case > 5 and <= 10:
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanFiveDay)?.Percentage ?? 0;
                    break;
                case > 10 and <= 15:
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanTenDay)?.Percentage ?? 0;
                    break;
                case > 15:
                    employeeReportViewDto.Discount += incentiveAndDiscounts
                        .FirstOrDefault(i => i.Type == IncentiveAndDiscountType.DiscountOnAbsentMoreThanFifteenDay)?.Percentage ?? 0;
                    break;
            }
        }

        private static void CalculateTotalSalary(EmployeeReportViewDto employeeReportViewDto)
        {
            employeeReportViewDto.TotalSalary = employeeReportViewDto.BaseSalary +
                (employeeReportViewDto.BaseSalary * employeeReportViewDto.Incentive) / 100 -
                (employeeReportViewDto.BaseSalary * employeeReportViewDto.Discount) / 100;
        }
    }
}
