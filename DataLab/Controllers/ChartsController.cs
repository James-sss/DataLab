using DataLab.IServices;
using DataLab.ViewModels.Chart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    [Authorize(Roles = "Role_Admin")]
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



        [HttpGet]
        public async Task<IActionResult> GetCharts( int id)
        {
            var customer = await _customerService.GetCustomerByid(id);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }


            ViewBag.CustomerId = customer.CustomerId;
            ViewBag.CustomerName = customer.CustomerName;
            ViewBag.DataSource = customer.DataSource;


            string SensorTypelist;
            string Monthlist;
            string AverageValueslist;
            string MinimumValueslist;
            string MaxmumValueslist;
            string TotalFilesCountlist;


            var GetSensorDataResults = _dataService.GetAllDataByCustId(customer.CustomerId).Select(e => new
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

            var getyears = _dataService.GetAllDataByCustId(customer.CustomerId).Select(e => new
            {
                DateTime = e.Datetime,
            }).OrderByDescending(e => e.DateTime?.Year).ToList();

            ViewBag.firstYear = getyears.Select(e => e.DateTime?.ToString("MM/dd/yyyy")).Last();
            ViewBag.lastYear = getyears.Select(e => e.DateTime?.ToString("MM/dd/yyyy")).First();

            return View();
        }
    }
}
