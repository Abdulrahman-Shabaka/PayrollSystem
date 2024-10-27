using PayrollSystem.Interfaces;
using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Services;

public class AttendanceService(IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<AttendanceReportDto>> GetAttendanceReport(int? month, int? year, int? employeeId)
    {
        return await unitOfWork.Attendances.GetAttendanceReport(month, year, employeeId);
    }
    public async Task<IEnumerable<Attendance>> GetFilteredAttendancesAsync(int? employeeId, DateTime? date)
    {
        return await unitOfWork.Attendances.GetFilteredAttendancesAsync(employeeId, date);
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await unitOfWork.Attendances.GetByIdAsync(id);
    }

    public async Task AddAttendanceAsync(Attendance attendance)
    {
        await unitOfWork.Attendances.AddAsync(attendance);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateAttendanceAsync(Attendance attendance)
    {
        unitOfWork.Attendances.Update(attendance);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteAttendanceAsync(int id)
    {
        await unitOfWork.Attendances.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }

    public async Task<bool> CheckExistenceAsync(int employeeId, DateTime date)
    {
        return await unitOfWork.Attendances.CheckExistenceAsync(employeeId, date);
    }
}