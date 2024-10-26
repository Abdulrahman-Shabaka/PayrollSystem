using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;
using PayrollSystem.Services;

namespace PayrollSystem.Controllers;

public class EmployeeController(EmployeeService employeeService, DepartmentService departmentService, AttendanceService attendanceService, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index()
    {
        var employees = await employeeService.GetWithRelatedDate();
        var employeesDto = mapper.Map<List<EmployeeDto>>(employees);
        return View(employeesDto);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeDto employeeDto)
    {
        if (ModelState.IsValid)
        {
            var employee = mapper.Map<Employee>(employeeDto);
            await employeeService.AddEmployeeAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
        return View(employeeDto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await employeeService.GetByIdAsync(id);
        if (employee == null) return NotFound();

        var employeeDto = mapper.Map<EmployeeDto>(employee);

        ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
        return View(employeeDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeDto employeeDto)
    {
        if (ModelState.IsValid)
        {
            var employee = mapper.Map<Employee>(employeeDto);
            await employeeService.UpdateEmployeeAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Departments = await departmentService.GetAllDepartmentsAsync();
        return View(employeeDto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await employeeService.DeleteEmployeeAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Login(int id)
    {
        var attendance = new Attendance()
        {
            Date = DateTime.Now,
            EmployeeId = id,
            LoginTime = DateTime.Now,
            Status = AttendanceStatus.Present
        };

        await attendanceService.AddAttendanceAsync(attendance);


        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Logout(int id)
    {
        var attendance = await attendanceService.GetByIdAsync(id);
        if (attendance == null) return NotFound();

        attendance.LogoutTime = DateTime.Now;

        await attendanceService.UpdateAttendanceAsync(attendance);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Absent(int id)
    {
        var attendance = new Attendance()
        {
            Date = DateTime.Now,
            EmployeeId = id,
            Status = AttendanceStatus.Absent
        };

        await attendanceService.AddAttendanceAsync(attendance);

        return RedirectToAction(nameof(Index));
    }
}