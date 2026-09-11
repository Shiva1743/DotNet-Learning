using CoreEmptyProject1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreEmptyProject1.Controllers
{
    //public class HomeController
    //{
    //    public string Index()
    //    {
    //        return "hello hi from mvc";
    //    }
    //}

    public class HomeController : Controller // for core methods
    {
        private IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository)// constructor injection
        {
            _employeeRepository = employeeRepository;
        }
        public string Index()
        {
             return _employeeRepository.GetEmployee(1).Name;
            //return Json(new { id = 1, name = "shiv" });
        }

        public ViewResult Details()
        {
            Employee model = _employeeRepository.GetEmployee(1);
            //return View(model);

            //return View("../../MyViews/Test");
            //return View("../Test/update");
            return View("Test");
            //return new ObjectResult(model);   
        }

    }
}
