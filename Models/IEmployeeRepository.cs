namespace CoreEmptyProject1.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployee();
        Employee AddEmp(Employee employee);
        Employee? Delete(int id);
        Employee? UpdateEmp(Employee employeeChanges);
    }
}
