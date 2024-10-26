using Microsoft.AspNetCore.Mvc;

using PayrollSystem.Models.Domain;
using PayrollSystem.Services;

namespace PayrollSystem.Controllers
{
    public class IncentiveAndDiscountController(IncentiveAndDiscountService incentiveAndDiscountService, DepartmentService departmentService) : Controller
    { 
        public async Task<IActionResult> Index()
        {
            var incentives = await incentiveAndDiscountService.GetAllIncentivesAsync();
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
            if (await incentiveAndDiscountService.CheckExistenceAsync(incentiveAndDiscount.Type, incentiveAndDiscount.DepartmentId))
            {
                TempData["Message"] = "A Value for this Incentive and Discount Type already exists.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                await incentiveAndDiscountService.AddIncentiveAsync(incentiveAndDiscount);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();

            return View(incentiveAndDiscount);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var incentive = await incentiveAndDiscountService.GetByIdAsync(id);
            if (incentive == null) return NotFound();

            ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
            return View(incentive);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IncentiveAndDiscount incentiveAndDiscount)
        {
            if (ModelState.IsValid)
            {
                await incentiveAndDiscountService.UpdateIncentiveAsync(incentiveAndDiscount);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
            return View(incentiveAndDiscount);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await incentiveAndDiscountService.DeleteIncentiveAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
