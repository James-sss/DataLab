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
    }
}
