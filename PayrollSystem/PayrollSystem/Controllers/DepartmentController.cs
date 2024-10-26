using Microsoft.AspNetCore.Mvc;
using PayrollSystem.Models.Domain;
using PayrollSystem.Services;

namespace PayrollSystem.Controllers;

public class DepartmentController(DepartmentService departmentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var departments = await departmentService.GetAllDepartmentsAsync();
        return View(departments);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Department department)
    {
        if (await departmentService.CheckExistenceAsync(department.Name))
        {
            TempData["Message"] = "A Value for this Incentive and Discount Type already exists.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid) return View(department);
        await departmentService.AddDepartmentAsync(department);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var department = await departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return View(department);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Department department)
    {
        if (ModelState.IsValid)
        {
            await departmentService.UpdateDepartmentAsync(department);
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await departmentService.DeleteDepartmentAsync(id);
        return RedirectToAction(nameof(Index));
    }
}