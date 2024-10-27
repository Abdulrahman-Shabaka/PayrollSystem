using System.ComponentModel.DataAnnotations;

namespace PayrollSystem.Models.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        [Required] public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required] public required string Address { get; set; }
        [Required] [Phone] public required string Phone { get; set; }
        [EmailAddress] public string? Email { get; set; }
        public JobGrade JobGrade { get; set; }
        public int DepartmentId { get; set; }
        public DateTime HireDate { get; set; }

        public virtual required Department Department { get; set; } 
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
