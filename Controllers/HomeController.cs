using CoreEmptyProject1.Models;
using CoreEmptyProject1.ViewModels;
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
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
            //return Json(new { id = 1, name = "shiv" });
            //Employee model = _employeeRepository.GetEmployee(1);
            //ViewData["Emp"] = model;s
            //ViewData["PageTitle"] = "Employee Details";

            //return View(ViewData);
        }

        //[Route("Home/Details/{id?}")]
        public ViewResult Details(int? id)
        {

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Empp = _employeeRepository.GetEmployee(id??3),
                PageTitle = "Employee Details",
            };
            return View(homeDetailsViewModel);

            //Employee model = _employeeRepository.GetEmployee(id??3);
            //ViewBag.PageTitle = "Employee Details";
            //return View(model);

            //// For ViewBag = loosly typed data display in view page,doesnt shows complile time error,shows run time error only
            //ViewBag.PageTitle = "Employee Details";
            //ViewBag.Emp = model;
            //return View(ViewBag);

            ////For ViewData = loosly typed data display in view page
            //ViewData["PageTitle"] = "Employee Details";
            //ViewData["Emp"] = model;
            //return View(ViewData);
        }

        //{
        //        Employee model = _employeeRepository.GetEmployee(1);
        //        //return View(model);
        //        //return View("../../MyViews/Test");
        //        //return View("../Test/update");
        //        return View("Test");
        //    //return new ObjectResult(model);   
        //}

    }
}
