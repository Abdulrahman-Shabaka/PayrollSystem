using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Models.Domain;

public class IncentiveAndDiscount
{
    public int Id { get; set; }
    public IncentiveAndDiscountCategory Category { get; set; }
    public IncentiveAndDiscountType Type { get; set; }

    [Range(0, 100)]
    public decimal Percentage { get; set; }

    public int? MinimumYears { get; set; }
    public int? MaximumYears { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
}