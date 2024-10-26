using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PayrollSystem.Models.Domain;
using PayrollSystem.Services;

namespace PayrollSystem.Controllers
{
    public class SalaryController(SalaryService salaryService) : Controller
    { 
        public async Task<IActionResult> Index()
        {
            var salaries = await salaryService.GetAllSalariesAsync();
            return View(salaries);
        }

        public async Task<IActionResult> Create()
        {
            var existingSalaries = await salaryService.GetAllSalariesAsync();
            ViewBag.ExistingSalaries = existingSalaries;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Salary salary)
        {
            var isExist = await salaryService.CheckExistenceAsync(salary.JobGrade);
            if (isExist) ModelState.AddModelError("JobGrade", "A salary for this job grade already exists.");
            
            if (!ModelState.IsValid) return View(salary);
            await salaryService.AddSalaryAsync(salary);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var salary = await salaryService.GetByIdAsync(id);
            if (salary == null) return NotFound();

            return View(salary);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Salary salary)
        {
            if (!ModelState.IsValid) return View(salary);
            await salaryService.UpdateSalaryAsync(salary);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            await salaryService.DeleteSalaryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
