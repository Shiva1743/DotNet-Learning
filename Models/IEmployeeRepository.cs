namespace CoreEmptyProject1.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployee();
        Employee AddEmp(Employee employee);
    }
}
