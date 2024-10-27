using AutoMapper;

using PayrollSystem.Models.Domain;
using PayrollSystem.Models.Dtos;

namespace PayrollSystem.Models.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Attendances, opt => opt.Ignore());

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.Attendance, opt => opt.MapFrom(src => src.Attendances.FirstOrDefault()));

        CreateMap<EmployeeReportResponseDto, EmployeeReportViewDto>();

        CreateMap<Attendance, AttendanceDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name));

        CreateMap<AttendanceDto, Attendance>()
            .ForMember(dest => dest.Employee, opt => opt.Ignore());
    }
}