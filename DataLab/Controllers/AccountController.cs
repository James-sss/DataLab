﻿using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.ViewModels.Authentication;
using DataLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotyfService _toastNotification;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 INotyfService toastNotification)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _toastNotification = toastNotification;
        }



        [HttpGet]
        public IActionResult Register()
        {
            var modelVM = new RegisterVM()
            {
                ApplicationUserList = _userManager.Users
            };
            return View(modelVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    AccountType = model.AccountType
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("testpage", "Account");

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("testpage", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult DemoLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DemoLogin(DemoLoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("testpage", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailAvailability(string email)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }



        [HttpGet]
        public async Task<IActionResult> EditRegisteredUser(string Id)
        {

            var RegisteredUser = await _userManager.FindByIdAsync(Id);

            if (RegisteredUser == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }
            else
            {
                
                var model = new EditRegisteredUserVM
                {

                    FirstName = RegisteredUser.FirstName,
                    LastName = RegisteredUser.LastName,
                    Email = RegisteredUser.Email,
                    AccountType = RegisteredUser.AccountType,
                };

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRegisteredUser(EditRegisteredUserVM model)
        {
            var RegisteredUser = await _userManager.FindByIdAsync(model.Id);

            if (RegisteredUser == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }
            else
            {
                RegisteredUser.FirstName = model.FirstName;
                RegisteredUser.LastName = model.LastName;
                RegisteredUser.Email = model.Email;
                RegisteredUser.AccountType = model.AccountType;

                var result = await _userManager.UpdateAsync(RegisteredUser);

                if (result.Succeeded)
                {
                    _toastNotification.Success($"User was updated successfully");
                    return RedirectToAction("Register");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteRegisteredUser(string Id)
        {

            var RegisteredUser = await _userManager.FindByIdAsync(Id);

            if (RegisteredUser == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }
            else
            {

                var result = await _userManager.DeleteAsync(RegisteredUser);

                if (result.Succeeded)
                {
                    _toastNotification.Success($"User was deleted successfully");
                    return RedirectToAction("Register");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Register");
            }
        }



        public IActionResult testpage()
        {
            return View();
        }
    }
}
