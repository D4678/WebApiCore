using System.ComponentModel.DataAnnotations;

namespace Ems_Project.Models
{
    public class EmployeeDto
    {
        [Required]
        public int ID { get; set; } // Primary Key, Identity
        [Required]
        public string FirstName { get; set; } // varchar(50)
        [Required]
        public string LastName { get; set; } // varchar(50)
        [Required]
        public string Gender { get; set; } // varchar(20)
        [Required]
        public int Salary { get; set; } // int
    }
}
