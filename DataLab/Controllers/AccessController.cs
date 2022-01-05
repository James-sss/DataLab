using DataLab.IServices;
using DataLab.Models;
using DataLab.ViewModels.Accsess;
using DataLab.ViewModels.Chart;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    public class AccessController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccessService _accessService;

        public AccessController(UserManager<ApplicationUser> userManager,
                                IAccessService accessService)
        {
            _userManager = userManager;
            _accessService = accessService;
        }


        public async Task<IActionResult> WelcomePage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var UserId = user?.Id;

            var GetResources = _accessService.ReturnResourcesByUserId(UserId).FirstOrDefault();
            if (GetResources == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var Alldata = _accessService.GetData(GetResources.CustomerId);

            var modelVm = new GetResourceVM();
            modelVm.CustomerId = GetResources.CustomerId;
            modelVm.CustomerName = GetResources.Customers.CustomerName;
            modelVm.DataSource = GetResources.Customers.DataSource;
            modelVm.CollectedData = Alldata;
            modelVm.AuthUsers = _accessService.ReturnAuthorizedUsers(GetResources.CustomerId);

           
            string SensorTypelist;
            string TotalFilesCountlist;

            var GetSensorDataResults = Alldata.Select(e => new
            {
                SensorTypes = e.SensorTypes.SensorType,
                datacount = e.Customers.CollectedData

            }).GroupBy(x => new { x.SensorTypes }, (key, group) => new ChartVM
            {
                SensorType = key.SensorTypes,
                TotalFilesCount = group.Count(x => x.datacount.Any()),
            }).ToList();

            List<ChartVM> SensorResultsList = new List<ChartVM>();

            foreach (var CollectedSensorResult in GetSensorDataResults)
            {
                ChartVM SensorResults = new ChartVM();

                SensorResults.SensorType = CollectedSensorResult.SensorType;
                SensorResults.TotalFilesCount = CollectedSensorResult.TotalFilesCount;

                SensorResultsList.Add(SensorResults);

                SensorTypelist = "'" + string.Join("','", SensorResultsList.Select(n => n.SensorType).ToList()) + "'";
                TotalFilesCountlist = string.Join(",", SensorResultsList.Select(n => n.TotalFilesCount).ToList());

                ViewBag.sensorTypes = SensorTypelist;
                ViewBag.totalDataFiles = TotalFilesCountlist;
            }

            return View(modelVm);
        }



        [HttpGet]
        public async Task<IActionResult> CustomerData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var GetResources = _accessService.ReturnResourcesByUserId(user.Id).FirstOrDefault();
            var Alldata = _accessService.GetData(GetResources.CustomerId);

            var modelVM = new GetResourceVM
            {
                CollectedData = Alldata,
                CustomerName = GetResources.Customers.CustomerName,
                DataSource = GetResources.Customers.DataSource,
                CustomerId = GetResources.Customers.CustomerId
            };

            return View(modelVM);
        }



        [HttpGet]
        public async Task<IActionResult> CustomerCharts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var GetResources = _accessService.ReturnResourcesByUserId(user.Id).FirstOrDefault();
            var Alldata = _accessService.GetData(GetResources.CustomerId);


            ViewBag.CustomerId = GetResources.Customers.CustomerId;
            ViewBag.CustomerName = GetResources.Customers.CustomerName;
            ViewBag.DataSource = GetResources.Customers.DataSource;


            string SensorTypelist;
            string Monthlist;
            string AverageValueslist;
            string MinimumValueslist;
            string MaxmumValueslist;
            string TotalFilesCountlist;


            var GetSensorDataResults = Alldata.Select(e => new
            {
                Datetime = e.Datetime.Value,
                SensorTypes = e.SensorTypes.SensorType,
                Value = e.Value

            }).OrderBy(e => e.Datetime.Month).GroupBy(x => new { x.SensorTypes, month = x.Datetime.ToString("MMMM") }, (key, group) => new ChartVM
            {
                SensorType = key.SensorTypes,
                Month = key.month,
                AverageValues = group.Count() > 0 ? group.Average(x => x.Value) : 0,
                MinimumValues = group.Min(x => x.Value),
                MaxmumValues = group.Max(x => x.Value),
                TotalFilesCount = group.Count(x => x.SensorTypes.Any())
            }).OrderBy(e => e.SensorType).ToList();


            List<ChartVM> SensorResultsList = new List<ChartVM>();

            foreach (var CollectedSensorResult in GetSensorDataResults)
            {
                ChartVM SensorResults = new ChartVM();

                SensorResults.SensorType = CollectedSensorResult.SensorType;
                SensorResults.Month = CollectedSensorResult.Month;
                SensorResults.AverageValues = (int)CollectedSensorResult.AverageValues;
                SensorResults.MinimumValues = (int)CollectedSensorResult.MinimumValues;
                SensorResults.MaxmumValues = (int)CollectedSensorResult.MaxmumValues;
                SensorResults.TotalFilesCount = CollectedSensorResult.TotalFilesCount;

                SensorResultsList.Add(SensorResults);


                SensorTypelist = "'" + string.Join("','", SensorResultsList.Select(n => n.SensorType).ToList()) + "'";
                Monthlist = string.Join(", ", SensorResultsList.Select(n => n.Month).Distinct().ToList());
                AverageValueslist = string.Join(",", SensorResultsList.Select(n => n.AverageValues).ToList());
                MinimumValueslist = string.Join(",", SensorResultsList.Select(n => n.MinimumValues).ToList());
                MaxmumValueslist = string.Join(",", SensorResultsList.Select(n => n.MaxmumValues).ToList());
                TotalFilesCountlist = string.Join(",", SensorResultsList.Select(n => n.TotalFilesCount).ToList());


                ViewBag.sensorTypes = SensorTypelist;
                ViewBag.months = Monthlist;
                ViewBag.AverageValues = AverageValueslist;
                ViewBag.MinimumValues = MinimumValueslist;
                ViewBag.MaxmumValues = MaxmumValueslist;
                ViewBag.TotalDataFiles = TotalFilesCountlist;
            }

            var getyears = Alldata.Select(e => new
            {
                DateTime = e.Datetime,
            }).OrderByDescending(e => e.DateTime?.Year).ToList();

            ViewBag.firstYear = getyears.Select(e => e.DateTime?.ToString("MM/dd/yyyy")).Last();
            ViewBag.lastYear = getyears.Select(e => e.DateTime?.ToString("MM/dd/yyyy")).First();

            //Testing results with json
            //return Json(GetSensorDataResults);

            return View();
        }

    }
}
