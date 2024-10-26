using Microsoft.EntityFrameworkCore;

namespace PayrollSystem.Models.Domain;

public class Salary
{
    public int Id { get; set; }
    public decimal BaseSalary { get; set; }
    public JobGrade JobGrade { get; set; }
}