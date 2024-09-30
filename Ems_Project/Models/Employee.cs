using System.ComponentModel.DataAnnotations;

namespace Ems_Project.Models
{
    public class Employee
    {
       
        public int ID { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Gender { get; set; } 
        public int Salary { get; set; }

        internal static void Add(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
