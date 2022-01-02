using DataLab.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    public class ChartsController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ICustomerService _customerService;

        public ChartsController(IDataService dataService,
                                ICustomerService customerService)
        {
            _dataService = dataService;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
