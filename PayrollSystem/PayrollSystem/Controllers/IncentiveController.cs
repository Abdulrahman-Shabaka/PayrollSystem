using Microsoft.AspNetCore.Mvc;

using PayrollSystem.Models.Domain;
using PayrollSystem.Services;

namespace PayrollSystem.Controllers
{
    public class IncentiveController(IncentiveService incentiveService, DepartmentService departmentService) : Controller
    { 
        public async Task<IActionResult> Index()
        {
            var incentives = await incentiveService.GetAllIncentivesAsync();
            return View(incentives);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IncentiveAndDiscount incentiveAndDiscount)
        {
            if (ModelState.IsValid)
            {
                await incentiveService.AddIncentiveAsync(incentiveAndDiscount);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
            return View(incentiveAndDiscount);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var incentive = await incentiveService.GetByIdAsync(id);
            if (incentive == null) return NotFound();

            ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
            return View(incentive);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IncentiveAndDiscount incentiveAndDiscount)
        {
            if (ModelState.IsValid)
            {
                await incentiveService.UpdateIncentiveAsync(incentiveAndDiscount);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
            return View(incentiveAndDiscount);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await incentiveService.DeleteIncentiveAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
