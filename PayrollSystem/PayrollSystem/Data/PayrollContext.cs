using Microsoft.EntityFrameworkCore;

using PayrollSystem.Models.Domain;

namespace PayrollSystem.Data;

public class PayrollContext(DbContextOptions<PayrollContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<IncentiveAndDiscount> Incentives { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Salary>()
            .HasIndex(s => s.JobGrade)
            .IsUnique();
    }
}