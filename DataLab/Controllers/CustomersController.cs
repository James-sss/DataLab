using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.IServices;
using DataLab.Models;
using DataLab.ViewModels.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    [Authorize(Roles = "Role_Admin")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly INotyfService _toastNotification;
        private readonly IAuthUserService _authUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomersController(ICustomerService customerService,
                                   INotyfService toastNotification,
                                   IAuthUserService authUserService,
                                   UserManager<ApplicationUser> userManager)
        {
            _customerService = customerService;
            _toastNotification = toastNotification;
            _authUserService = authUserService;
            _userManager = userManager;
        }



        [HttpGet]
        public IActionResult AddCustomer()
        {
            var modelVM = new AddCustomerVM()
            {
                CustomerList = _customerService.GetAllCustomers()
            };
            return View(modelVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerVM modelVM)
        {
            if (ModelState.IsValid)
            {

                Customers farm = new Customers
                {
                    CustomerName = modelVM.CustomerName,
                    Address = modelVM.Address,
                    DataSource = modelVM.DataSource,
                };

                await _customerService.AddCustomer(farm);

                _toastNotification.Success($"Customer {modelVM.CustomerName} was successfully added ");
                return RedirectToAction("AddCustomer");
            }
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> EditCustomer(int Id)
        {
            var customer = await _customerService.GetCustomerByid(Id);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            EditCustomerVM customerModel = new EditCustomerVM
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                DataSource = customer.DataSource,
                Address = customer.Address,
                AuthorizedUsersList = _authUserService.GetAuthUsersList(Id)
            };

            return View(customerModel);
        }

        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> EditCustomer(EditCustomerVM modelVM)
        {
            if (ModelState.IsValid)
            {

                var customer = await _customerService.GetCustomerByid(modelVM.CustomerId);

                if (customer == null)
                {
                    Response.StatusCode = 404;
                    return View("DashBordNotFoundErros");
                }

                customer.CustomerName = modelVM.CustomerName;
                customer.DataSource = modelVM.DataSource;
                customer.Address = modelVM.Address;

                await _customerService.UpdateCustomer(customer);

                _toastNotification.Success($"Customer {modelVM.CustomerName} was successfully updated ");
                return RedirectToAction("AddCustomer");
            }
            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> DeleteCustomer(int Id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByid(Id);

                if (customer == null)
                {
                    Response.StatusCode = 404;
                    return View("DashBordNotFoundErros");
                }

                await _customerService.DeleteCustomer(customer);

                _toastNotification.Success($"Customer {customer.CustomerName} was successfully deleted ");
                return RedirectToAction("AddCustomer");

            }
            catch (DbUpdateException)
            {
                _toastNotification.Warning("Error!! Could not delete. Please try again, and if the problem persists contact us at Data-Lab.Info.com");
                return RedirectToAction("AddCustomer");
            }
        }



        [HttpGet]
        public async Task<IActionResult> AssignUserToCust(int Id)
        {
            var customer = await _customerService.GetCustomerByid(Id);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            List<ApplicationUser> listOfUserAssigned = new List<ApplicationUser>();
            List<ApplicationUser> listOfUserNotAssigned = new List<ApplicationUser>();


            var AllApplicationUsersList = _userManager.Users;

            foreach (ApplicationUser user in AllApplicationUsersList.ToList())
            {
                var list = _authUserService.IsUserAssigned(user, customer.CustomerId) ? listOfUserAssigned : listOfUserNotAssigned;
                list.Add(user);
            }

            var modelVM = new AssignUserToCustVM
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                DataSource = customer.DataSource,
                ListUsersAssigned = listOfUserAssigned,
                ListUsersNotAssigned = listOfUserNotAssigned,
            };
            return View(modelVM);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserToCust(AssignUserToCustVM modelVM, string[] ListUsersToAdd, string[] ListUserstoRemove)
        {
            bool Action_Add = false;

            foreach (string selectedId in ListUsersToAdd)
            {
                AuthorizedUsers user = new AuthorizedUsers();
                user.CustomerId = modelVM.CustomerId;
                user.UserId = selectedId;

                await _authUserService.AddAuthUsers(user);

                Action_Add = true;
            }

            foreach (string selectedId in ListUserstoRemove)
            {
                AuthorizedUsers user = new AuthorizedUsers();
                user.CustomerId = modelVM.CustomerId;
                user.UserId = selectedId; 

                await _authUserService.RemoveAuthUsers(user);
            }

            if (Action_Add == true)
            {
                _toastNotification.Success($"User was assigned successfully");
                return RedirectToAction("EditCustomer", new { id = modelVM.CustomerId });
            }

            _toastNotification.Success($"User was removed successfully");
            return RedirectToAction("EditCustomer", new { id = modelVM.CustomerId });

        }
    }
}
