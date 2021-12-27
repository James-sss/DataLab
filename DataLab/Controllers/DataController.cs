using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.IServices;
using DataLab.ViewModels.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    public class DataController : Controller
    {
        private readonly IDataService _dataService;
        private readonly INotyfService _toastNotification;
        private readonly ICustomerService _customerService;

        public DataController(IDataService dataService,
                              INotyfService toastNotification,
                              ICustomerService customerService)
        {
            _dataService = dataService;
            _toastNotification = toastNotification;
            _customerService = customerService;
        }


        public async Task<IActionResult> FetchData(int id)
        {
            var customer = await _customerService.GetCustomerByid(id);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var modelVM = new FetchDataVM
            {
                CustomerName = customer.CustomerName,
                DataSource = customer.DataSource,
                Address = customer.Address,
                CollectedData = _dataService.GetAllDataByCustId(customer.CustomerId),
                
            };

            return View(modelVM);
        }
    }
}
