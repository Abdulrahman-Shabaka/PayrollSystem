using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Models.Domain;

public class Department
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public virtual ICollection<IncentiveAndDiscount> IncentiveAndDiscounts { get; set; } = new List<IncentiveAndDiscount>();
}