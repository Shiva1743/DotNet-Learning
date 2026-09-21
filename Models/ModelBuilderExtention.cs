using Microsoft.EntityFrameworkCore;

namespace CoreEmptyProject1.Models
{
    public static class ModelBuilderExtention
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 1,
                   Name = "shivani",
                   Email = "test@gmail.com",
                   Department = Dept.HR
               },
               new Employee
               {
                   Id = 2,
                   Name = "ajay",
                   Email = "test@gmail.com",
                   Department = Dept.Payroll
               },
               new Employee
               {
                   Id = 3,
                   Name = "meera",
                   Email = "meera@gmail.com",
                   Department = Dept.Sale
               }
            );

        }
    }
}
