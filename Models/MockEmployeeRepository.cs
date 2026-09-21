
namespace CoreEmptyProject1.Models
{
     //In Memory Storing Repository
    public class MockEmployeeRepository : IEmployeeRepository
    {

        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>() {
                new Employee() { Id=1, Name="shiv" , Email="abc@gmail.com", Department=Dept.HR},
                new Employee() { Id=2, Name="shivi" , Email="test@gmail.com", Department=Dept.IT},
                new Employee() { Id=3, Name="meera" , Email="test1@gmail.com", Department=Dept.Sale},
                new Employee() { Id=4, Name="ajay" , Email="test2@gmail.com", Department=Dept.Sale},
            };
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            //throw new NotImplementedException();
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            //throw new NotImplementedException();
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }
        public Employee AddEmp(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
            //throw new NotImplementedException();
        }

        public Employee? Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null) {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public Employee? UpdateEmp(Employee employeeChanges)
        {
            Employee? empData = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (empData != null)
            {
                empData.Name = employeeChanges.Name;
                empData.Email = employeeChanges.Email;
                empData.Department = employeeChanges.Department;
            }
            return empData;
        }
    }
}
