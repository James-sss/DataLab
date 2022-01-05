using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.Models;
using DataLab.ViewModels.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    [Authorize(Roles = "Role_Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _toastNotification;

        public RolesController(RoleManager<IdentityRole> roleManager,
                                 UserManager<ApplicationUser> userManager,
                                 INotyfService toastNotification)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }



        [HttpGet]
        public IActionResult CreateUserRole()
        {
            var modelVM = new UserRolesVM()
            {
                IdentityRole = _roleManager.Roles
            };
            return View(modelVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRole(UserRolesVM model)
        {

            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    _toastNotification.Success($"Role {model.RoleName} Was Successfully Created ");
                    return RedirectToAction("CreateUserRole");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> EditUserRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            if (role == null)
            {

                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            List<ApplicationUser> listUsersInRole = new List<ApplicationUser>();
            List<ApplicationUser> listUsersnotInRole = new List<ApplicationUser>();

            foreach (ApplicationUser user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? listUsersInRole : listUsersnotInRole;
                list.Add(user);
            }

            var model = new EditRoleVM
            {
                Id = role.Id,
                RoleName = role.Name,
                UsersInRoleList = listUsersInRole
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> EditUserRole(EditRoleVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    _toastNotification.Success($"RoleName {model.RoleName} Was Successfully Edited ");
                    return RedirectToAction("CreateUserRole");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("CreateUserRole");
            }
        }



        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var userRole = await _roleManager.FindByIdAsync(Id);
            if (userRole == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var result = await _roleManager.DeleteAsync(userRole);
            if (result.Succeeded)
            {
                _toastNotification.Success($"Role was successfully deleted");
                return RedirectToAction("CreateUserRole");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("CreateUserRole");
        }



        [HttpGet]
        public async Task<IActionResult> AddRoleToUser(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            List<IdentityRole> ListRolesAssignedToUser = new List<IdentityRole>();
            List<IdentityRole> ListRolesNotAssignedToUser = new List<IdentityRole>();

            foreach (IdentityRole role in _roleManager.Roles.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? ListRolesAssignedToUser : ListRolesNotAssignedToUser;
                list.Add(role);
            }

            var modelVM = new AddRoleToUserVM
            {
                UserId = user.Id,
                FullNames = user.FullName,
                RolesAssignedToUser = ListRolesAssignedToUser,
                RolesNotAssignedToUser = ListRolesNotAssignedToUser
            };
            return View(modelVM);
        }

        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserVM modelVM)
        {

            var user = await _userManager.FindByIdAsync(modelVM.UserId);

            if (user == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var CurrentUserRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = modelVM.ListRolesToAddORRemove.Except(CurrentUserRoles);
            var removedRoles = CurrentUserRoles.Except(modelVM.ListRolesToAddORRemove);

            await _userManager.AddToRolesAsync(user, selectedRoles);

            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            _toastNotification.Success($"Record was successfully modified ");
            return RedirectToAction("AddRoleToUser");

        }
    }
}
