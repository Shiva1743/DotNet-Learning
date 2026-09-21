using CoreEmptyProject1.Models;
using CoreEmptyProject1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoreEmptyProject1.Controllers
{
    //[Route("Home")]
    [Route("[controller]/[action]")]
    public class HomeController : Controller // for core methods
    {
        private IEmployeeRepository _employeeRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        // constructor injection
        public HomeController(IEmployeeRepository employeeRepository,
            IWebHostEnvironment hostingEnvironment,ILogger<HomeController> logger)
        {   
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        //[Route("")]
        //[Route("home")]
        //[Route("home/index")]
        //[Route("Index")]
        [Route("~/")]
        public ViewResult Index() // function name is not Index here so view should have the related cshtml file.
        {
            var model = _employeeRepository.GetAllEmployee();
            return View("~/Views/Home/Index.cshtml", model);
            //return View(model);

            //return Json(new { id = 1, name = "shiv" });
            //Employee model = _employeeRepository.GetEmployee(1);
            //ViewData["Emp"] = model;
            //ViewData["PageTitle"] = "Employee Details";

            //return View(ViewData);
        }

        //[Route("Home/Details/{id?}")]
        //[Route("Details/{id?}")]
        [Route("{id?}")]
        public ViewResult Details(int? id)
        {

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Empp = _employeeRepository.GetEmployee(id ?? 3),
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

        [HttpGet]
        public ViewResult Create()
        {
            Console.WriteLine("hi  from create");
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadPathFolder = Path.Combine(_hostingEnvironment.WebRootPath,"images");
                    uniqueFileName = Guid.NewGuid().ToString()+'_'+model.Photo.FileName;
                    string filePath = Path.Combine(uploadPathFolder, uniqueFileName);
                    _logger.LogInformation("File path = {Path}", filePath);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Employee newEmp = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    Photopath = uniqueFileName,
                };
                _employeeRepository.AddEmp(newEmp);
                return RedirectToAction("details",new {id= newEmp.Id});
            }
            return View();
        }
    }
}
