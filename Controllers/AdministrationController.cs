using CoreEmptyProject1.Models;
using CoreEmptyProject1.ViewModels;
using CoreEmptyProject1.Views.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace CoreEmptyProject1.Controllers
{
    [Authorize(Roles = "Admin")] // role shoulde either Admin or User
                                 //[Authorize(Roles= "Admin,User")] // role shoulde either Admin or User
                                 //[Authorize(Roles = "Admin")]
                                 //[Authorize(Roles = "User")] // role shoulde Admin and User both
    [Route("[controller]/[action]")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            //,ILogger<AdministrationController> logger
            )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this._signInManager = signInManager;
            //this.logger = logger;
        }

        [HttpGet]
        public ViewResult ListUsers()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User id  {id} not found";
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            //userManager.GetClaimsAsync(user);
            var model = new EditUserViewModel
            {
                Id = id,
                Email = user.Email,
                City = user.City,
                UserName = user.UserName,
                Claims = userClaims.Select(x => x.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }


        [HttpPost]
        [Route("{id?}")]
        public async Task<IActionResult> EditUser(EditUserViewModel model, string id)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User id  {model.Id} not found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.City = model.City;
                user.UserName = model.UserName;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Roles ane Claims form ma aavta nathi, etle fari bharo
            model.Roles = await userManager.GetRolesAsync(user);
            model.Claims = (await userManager.GetClaimsAsync(user)).Select(c => c.Value).ToList();

            return View(model);
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> ManageUserRoles(string id)
        {
            ViewBag.UserId = id;
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorTitle = $"User ID = {id}";
                ViewBag.ErrorMessage = $"User ID = {id} not found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }


        [HttpPost]
        [Route("{id?}")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorTitle = $"User ID = {id}";
                ViewBag.ErrorMessage = $"User ID = {id} not found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot remove user existing roles");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("EditUser", new { Id = id });
        }


        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> ManageUserClaims(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorTitle = $"User ID = {id}";
                ViewBag.ErrorMessage = $"User ID = {id} not found";
                return View("NotFound");
            }
            ViewBag.UserId = id;
            ViewBag.UserName = user.UserName;
            var existinguserClaims = await userManager.GetClaimsAsync(user);
            var model = new UserClaimsViewModel { UserId = user.Id, };
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaims userClaims = new UserClaims { ClaimType = claim.Type };
                if (existinguserClaims.Any(c => (c.Type == claim.Type)))
                {
                    userClaims.IsSelected = true;
                }
                model.Claims.Add(userClaims);
            }
            return View(model);
        }

        [HttpPost]
        [Route("{id?}")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model, string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorTitle = $"User ID = {id}";
                ViewBag.ErrorMessage = $"User ID = {id} not found";
                return View("NotFound");
            }
            var existinguserClaims = await userManager.GetClaimsAsync(user);
            model.UserId = user.Id;
            //remove all claims
            var result = await userManager.RemoveClaimsAsync(user, existinguserClaims);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove existing claims");
                return View(model);
            }
            result = await userManager.AddClaimsAsync(user, model.Claims.Where(x => x.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add claims");
                return View(model);
            }

            return RedirectToAction("EditUser",new {id = model.UserId});
        }

        [HttpPost]
        [Route("{id?}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"user id  {id} not found";
                return View("NotFound");
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("ListUsers");
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

            var editmodel = new EditRoleViewModel
            {
                Id = roleData.Id,
                RoleName = roleData.Name
            };
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                if (await userManager.IsInRoleAsync(user, roleData.Name))
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
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ViewResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            IdentityRole newidentityRole = new IdentityRole
            {
                Name = model.RoleName
            };
            IdentityResult result = await roleManager.CreateAsync(newidentityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles", "Administration");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("{id?}")]
        [Authorize(Policy= "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"role id  {id} not found";
                return View("NotFound");
            }
            try
            {
                //throw new Exception("test exception");
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");
            }
            catch (Exception ex)
            {
                //catch (DbUpdateException ex)
                ViewBag.ErrorTitle = $"{role.Name} role is in use.";
                ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted because role is already in use." +
                    $"If you want to delete this role, please remove user from this role and try to delete.";

                return View("NotFound");
            }
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
                    userRoleViewData.IsSelected = false;
                }
                model.Add(userRoleViewData);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ManageUsersInRole(List<UserRoleViewModel> model, string roleId)
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

                if (user != null && model[i].IsSelected && !(await userManager.IsInRoleAsync(user, roleData.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, roleData.Name);
                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, roleData.Name)))
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
                    return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
