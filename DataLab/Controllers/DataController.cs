using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.IServices;
using DataLab.Models;
using DataLab.ViewModels.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    [Authorize(Roles = "Role_Admin")]
    public class DataController : Controller
    {
        private readonly IDataService _dataService;
        private readonly INotyfService _toastNotification;
        private readonly ICustomerService _customerService;
        private readonly ISensorService _sensorService;

        public DataController(IDataService dataService,
                              INotyfService toastNotification,
                              ICustomerService customerService,
                              ISensorService sensorService)
        {
            _dataService = dataService;
            _toastNotification = toastNotification;
            _customerService = customerService;
            _sensorService = sensorService;
        }



        public async Task<IActionResult> FetchData(int id)
        {
            var customer = await _customerService.GetCustomerByid(id);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            ViewBag.customerId = customer.CustomerId;

            var modelVM = new FetchDataVM
            {
                CustomerName = customer.CustomerName,
                DataSource = customer.DataSource,
                Address = customer.Address,
                CollectedData = _dataService.GetAllDataByCustId(customer.CustomerId),
            };

            return View(modelVM);
        }



        [HttpGet]
        public async Task<IActionResult> AddData(int id)
        {
            var customer = await _customerService.GetCustomerByid(id);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            ViewBag.Customerid = customer.CustomerId;
            ViewBag.CustomerName = customer.CustomerName;
            ViewBag.Datasource = customer.DataSource;

            var SensorTypesSelectList = _sensorService.GetAllSensorTypes().Select(e => new SelectListItem()
            {
                Text = e.SensorType,
                Value = e.SensorTypeId.ToString()
            });

            ViewBag.SensorTypesSelectList = new SelectList(SensorTypesSelectList, "Value", "Text");

            var modelVM = new AddDataVM
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
            };

            return View(modelVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(AddDataVM modelVM)
        {
            var customer = await _customerService.GetCustomerByid(modelVM.CustomerId);

            if (customer == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var SensorTypesSelectList = _sensorService.GetAllSensorTypes().Select(e => new SelectListItem()
            {
                Text = e.SensorType,
                Value = e.SensorTypeId.ToString()
            });


            if (ModelState.IsValid)
            {

                CollectedData Data = new CollectedData
                {
                    Customers = customer,
                    Location = modelVM.Location,
                    Datetime = modelVM.Datetime,
                    SensorTypeId = modelVM.SensorTypeId,
                    Value = modelVM.Value


                };

                await _dataService.AddData(Data);

                _toastNotification.Success($"Data was added to customer successfully ");
                return RedirectToAction("AddData", new { id = modelVM.CustomerId });
            }

            ViewBag.SensorTypesSelectList = new SelectList(SensorTypesSelectList, "Value", "Text");

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> EditData(int id)
        {
            var data = await _dataService.GetDataByid(id);

            if (data == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            ViewBag.DataId = data.Id;
            ViewBag.CustomerId = data.CustomerId;

            var SensorTypesSelectList = _sensorService.GetAllSensorTypes().Select(e => new SelectListItem()
            {
                Text = e.SensorType,
                Value = e.SensorTypeId.ToString()
            });

            ViewBag.SensorTypesSelectList = new SelectList(SensorTypesSelectList, "Value", "Text");


            var modelVM = new EditDataVM
            {
                CollectedDataId = data.Id,
                CustomerId = data.CustomerId,
                Location = data.Location,
                Datetime = data.Datetime,
                SensorTypeId = data.SensorTypeId,
                Value = data.Value,
            };

            return View(modelVM);
        }

        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> EditData(EditDataVM modelVM)
        {
            var data = await _dataService.GetDataByid(modelVM.CollectedDataId);

            if (data == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            var SensorTypesSelectList = _sensorService.GetAllSensorTypes().Select(e => new SelectListItem()
            {
                Text = e.SensorType,
                Value = e.SensorTypeId.ToString()
            });


            if (ModelState.IsValid)
            {


                data.Id = modelVM.CollectedDataId;
                data.CustomerId = modelVM.CustomerId;
                data.Location = modelVM.Location;
                data.Datetime = modelVM.Datetime;
                data.SensorTypeId = modelVM.SensorTypeId;
                data.Value = modelVM.Value;


                await _dataService.UpdateData(data);

                _toastNotification.Success($"Data file with id {modelVM.CollectedDataId} was successfully updated ");
                return RedirectToAction("FetchData", new { id = modelVM.CustomerId });
            }

            ViewBag.SensorTypesSelectList = new SelectList(SensorTypesSelectList, "Value", "Text");

            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> DeleteData(int Id)
        {

            try
            {
                var data = await _dataService.GetDataByid(Id);

                if (data == null)
                {
                    Response.StatusCode = 404;
                    return View("DashBordNotFoundErros");
                }

                await _dataService.DeleteData(data);

                _toastNotification.Success($"Data file with id {data.Id} was successfully deleted ");
                return RedirectToAction("FetchData", new { id = data.CustomerId });

            }
            catch (DbUpdateException)
            {
                _toastNotification.Warning("Error!! Could not delete. Please try again, and if the problem persists contact us at Data-Lab.Info.com");
                return RedirectToAction("AddCustomer");
            }
        }


    }
}
