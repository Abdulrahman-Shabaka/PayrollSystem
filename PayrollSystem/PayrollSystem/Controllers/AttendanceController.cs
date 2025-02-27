using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;
using PayrollSystem.Models.ViewModels;
using PayrollSystem.Services;

namespace PayrollSystem.Controllers;

public class AttendanceController(AttendanceService attendanceService, EmployeeService employeeService, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index(int? employeeId, DateTime? date)
    {
        date ??= DateTime.Now;

        var attendances = await attendanceService.GetFilteredAttendancesAsync(employeeId, date);

        var viewModel = new AttendanceFilterViewModel
        {
            Attendances = attendances.ToList(),
            EmployeeId = employeeId,
            SelectedDate = date
        };

        ViewBag.Employees = await employeeService.GetAllEmployeesAsync();
        if (employeeId != null) ViewBag.SelectedEmployeeId = employeeId;

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Employees = await employeeService.GetAllEmployeesAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttendanceDto attendanceDto)
    {
        if(await attendanceService.CheckExistenceAsync(attendanceDto.EmployeeId, attendanceDto.Date))
        {
            TempData["Message"] = "The selected Employee have a record for this date";
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            var attendance = mapper.Map<Attendance>(attendanceDto);
            await attendanceService.AddAttendanceAsync(attendance);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Employees = await employeeService.GetAllEmployeesAsync();

        return View(attendanceDto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var attendance = await attendanceService.GetByIdAsync(id);
        if (attendance == null) return NotFound();

        var attendanceDto = mapper.Map<AttendanceDto>(attendance);
        return View(attendanceDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AttendanceDto attendanceDto)
    {
        if (!ModelState.IsValid) return View(attendanceDto);

        var attendance = mapper.Map<Attendance>(attendanceDto);
        await attendanceService.UpdateAttendanceAsync(attendance);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await attendanceService.DeleteAttendanceAsync(id);
        return RedirectToAction(nameof(Index));
    }
}