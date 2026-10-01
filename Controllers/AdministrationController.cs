using CoreEmptyProject1.Models;
using CoreEmptyProject1.ViewModels;
using CoreEmptyProject1.Views.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoreEmptyProject1.Controllers
{
    [Authorize(Roles="Admin,User")]
    [Route("[controller]/[action]")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager
            ,UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            //,ILogger<AdministrationController> logger
            )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this._signInManager = signInManager;
            //this.logger = logger;
        }

        [HttpGet]
        //[AllowAnonymous]
        public ViewResult ListRoles()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        [Route("{id?}")]
        [AllowAnonymous]
        public async Task<IActionResult> EditRole(string id)
        {
            var roleData = await roleManager.FindByIdAsync(id);
            if (roleData == null)
            {
                ViewBag.ErrorMessage = $"Role id  {id} not found";
                return View("NotFound");
            }

            var editmodel = new EditRoleViewModel {
                Id = roleData.Id,
                RoleName = roleData.Name
            };
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                if(await userManager.IsInRoleAsync(user, roleData.Name))
                {
                    editmodel.Users.Add(user.UserName);
                }
            }
            return View(editmodel);
        }

        [HttpPost]
        [Route("{id?}")]
        [AllowAnonymous]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            Helper.Dump(new { model });
            var roleData = await roleManager.FindByIdAsync(model.Id);
            if (roleData == null)
            {
                ViewBag.ErrorMessage = $"Role id  {model.Id} not found";
                return View("NotFound");
            }
            else
            {
                roleData.Id = model.Id;
                roleData.Name = model.RoleName;

                var result = await roleManager.UpdateAsync(roleData);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles","Administration");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    return View(model);
                }
            }
        }

        [HttpGet]
        public ViewResult CreateRole()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            IdentityRole newidentityRole = new IdentityRole {
                Name = model.RoleName
            };
            IdentityResult result = await roleManager.CreateAsync(newidentityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles", "Administration");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ManageUsersInRole(string roleId)
        {
            var roleData = await roleManager.FindByIdAsync(roleId);
            ViewBag.RoleId = roleId;
            if (roleData == null)
            {
                ViewBag.ErrorMessage = $"Role id  {roleId} not found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users.ToList())
            {
                var userRoleViewData = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, roleData.Name))
                {
                    userRoleViewData.IsSelected = true;
                }
                else
                {
                    userRoleViewData.IsSelected = false ;
                }
                model.Add(userRoleViewData);
            }
            return View(model);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ManageUsersInRole(List<UserRoleViewModel> model,string roleId)
        {
            var roleData = await roleManager.FindByIdAsync(roleId);
            ViewBag.RoleId = roleId;
            if (roleData == null)
            {
                ViewBag.ErrorMessage = $"Role id  {roleId} not found";
                return View("NotFound");
            }
             bool currentUserRoleRemoved = false;
             var currentUserId = userManager.GetUserId(User);

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if(user != null && model[i].IsSelected && !(await userManager.IsInRoleAsync(user, roleData.Name)))
                {
                    result = await userManager.AddToRoleAsync(user,roleData.Name);
                } else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, roleData.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, roleData.Name);
                    if (result.Succeeded && (user.Id == currentUserId))
                    {
                        currentUserRoleRemoved = true;
                    }
                }
                else
                {
                    continue;
                }
                if (!result.Succeeded)
                {
                    //return RedirectToAction("EditRole",new {Id = roleId});
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                if (currentUserRoleRemoved)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Login","Account");
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
