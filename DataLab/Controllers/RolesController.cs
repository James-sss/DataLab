using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.Models;
using DataLab.ViewModels.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
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
            return View();
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

    }
}
