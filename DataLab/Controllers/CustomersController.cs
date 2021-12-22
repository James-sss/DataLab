using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.IServices;
using DataLab.Models;
using DataLab.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly INotyfService _toastNotification;

        public CustomersController(ICustomerService customerService,
                                   INotyfService toastNotification)
        {
            _customerService = customerService;
            _toastNotification = toastNotification;
        }



        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
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
            };

            return View(customerModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFarm(EditCustomerVM modelVM)
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

    }
}
