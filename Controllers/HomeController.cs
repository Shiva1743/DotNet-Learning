using CoreEmptyProject1.Models;
using CoreEmptyProject1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

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
            IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
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
            //throw new Exception("hello show exceptioon error message here!!");
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Trace LogDebug");
            _logger.LogInformation("Trace LogInformation");
            _logger.LogWarning("Trace LogWarning");
            _logger.LogError("Trace LogError");
            _logger.LogCritical("Trace LogCritical");
            Employee EmppData = _employeeRepository.GetEmployee(id.Value);
            if (EmppData == null)
            {
                Response.StatusCode = 404;
                return View("NotFound",id.Value);
            }

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

        [HttpGet]
        [Route("{id?}")]
        public ViewResult Edit(int id)
        {
            _logger.LogInformation("========== EDIT ACTION CALLED ==========");

            Employee Empp = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel editDetailsViewModel = new EmployeeEditViewModel()
            {
                Name = Empp.Name,
                Email = Empp.Email,
                Department = Empp.Department,
                ExistingPhotoPath = Empp.Photopath,
            };
            return View(editDetailsViewModel);
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult edit(EmployeeEditViewModel model, int id)
        {
            //try
            //{
            //    _logger.LogInformation("Edit started. Id = {Id}", model.Id);

            //    _logger.LogInformation(
            //        "Model: Name={Name}, Email={Email}, Photo={Photo}",
            //        model.Name,
            //        model.Email,
            //        model.Photo?.FileName);

            if (ModelState.IsValid)
            {
                Employee Empp = _employeeRepository.GetEmployee(id);
                //Empp.Id = model.Id;
                Empp.Name = model.Name;
                Empp.Email = model.Email;
                Empp.Department = model.Department;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filepath);
                    }
                    Empp.Photopath = ProcessFileUploads(model);
                }
                _employeeRepository.UpdateEmp(Empp);
                return RedirectToAction("index");
            }
            return View(model);

            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error while updating employee.");

            //    throw;
            //}
        }

        private string ProcessFileUploads(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                //foreach (IFormFile photo in model.Photos)
                //{
                string uploadPathFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + '_' + model.Photo.FileName;
                string filePath = Path.Combine(uploadPathFolder, uniqueFileName);
                _logger.LogInformation("File path = {Path}", filePath);
                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(file);
                }

                //}
            }

            return uniqueFileName;
        }

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
                string uniqueFileName = ProcessFileUploads(model);
                Employee newEmp = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    Photopath = uniqueFileName,
                };
                _employeeRepository.AddEmp(newEmp);
                return RedirectToAction("details", new { id = newEmp.Id });
            }
            return View();
        }
    }
}
