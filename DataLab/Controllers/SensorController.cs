using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    public class SensorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
