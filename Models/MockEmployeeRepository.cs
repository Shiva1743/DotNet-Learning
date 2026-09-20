
namespace CoreEmptyProject1.Models
{
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

        public Employee GetEmployee(int Id)
        {
            //throw new NotImplementedException();
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }
        public Employee AddEmp(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
            //throw new NotImplementedException();
        }

    }
}
