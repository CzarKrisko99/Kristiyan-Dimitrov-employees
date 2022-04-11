
namespace Sirma_Solutions_Assigment.DTO.DTO
{
    public class Employee
    {
        public Employee()
        {
            this.Projects = new List<Project>();
        }

        public int EmployeeID { get; set; }

        public List<Project> Projects { get; set; }
    }
}
