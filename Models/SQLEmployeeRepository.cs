
namespace CoreEmptyProject1.Models
{
    //In SQL server database Storing Process
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Employee AddEmp(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee? Delete(int id)
        {
            Employee employeeData = context.Employees.FirstOrDefault(e => e.Id == id);
            if (employeeData != null)
            {
                context.Employees.Remove(employeeData);
            }
            return employeeData;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            Employee employeeData = context.Employees.FirstOrDefault(e => e.Id == id);
            return employeeData;
        }

        public Employee? UpdateEmp(Employee employeeChanges)
        {
            Employee? employeeData = context.Employees.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employeeData != null)
            {
                var emp = context.Employees.Attach(employeeChanges);
                emp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            return employeeChanges;
        }
    }
}
