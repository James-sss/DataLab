using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.IServices;
using DataLab.Models;
using DataLab.ViewModels.Sensor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Controllers
{
    [Authorize(Roles = "Role_Admin")]
    public class SensorController : Controller
    {
        private readonly ISensorService _sensorService;
        private readonly INotyfService _toastNotification;

        public SensorController(ISensorService sensorService,
                                INotyfService toastNotification)
        {
            _sensorService = sensorService;
            _toastNotification = toastNotification;
        }



        [HttpGet]
        public IActionResult AddSensor()
        {
            var modelVM = new AddSensorTypeVM()
            {
                ListSensorTypes = _sensorService.GetAllSensorTypes()
            };
            return View(modelVM);
        }
        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> AddSensor(AddSensorTypeVM modelVM)
        {

            if (ModelState.IsValid)
            {

                SensorTypes sensor = new SensorTypes
                {
                    SensorType = modelVM.SensorType,
                };
                await _sensorService.AddSensorType(sensor);

                _toastNotification.Success($"Sensor {modelVM.SensorType} was successfully added ");
                return RedirectToAction("AddSensor");
            }
            return View(modelVM);
        }



        [HttpGet]
        public async Task<IActionResult> EditSensor(int Id)
        {
            var sensor = await _sensorService.GetSensorTypeByid(Id);

            if (sensor == null)
            {
                Response.StatusCode = 404;
                return View("DashBordNotFoundErros");
            }

            ViewBag.SensorType = sensor.SensorType;

            EditSensorTypeVM sensorObject = new EditSensorTypeVM
            {
                SensorTypeId = sensor.SensorTypeId,
                SensorType = sensor.SensorType,
            };

            return View(sensorObject);
        }

        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> EditSensor(EditSensorTypeVM modelVM)
        {
            if (ModelState.IsValid)
            {

                var sensor = await _sensorService.GetSensorTypeByid(modelVM.SensorTypeId);

                if (sensor == null)
                {
                    Response.StatusCode = 404;
                    return View("DashBordNotFoundErros");
                }

                sensor.SensorType = modelVM.SensorType;

                await _sensorService.UpdateSensorType(sensor);

                _toastNotification.Success($"Sensor {modelVM.SensorType} was successfully updated ");
                return RedirectToAction("AddSensor");
            }
            return View(modelVM);
        }



        [HttpPost]
        [Authorize(Roles = "Role_SuperAdmin")]
        public async Task<IActionResult> DeleteSenser(int Id)
        {
            try
            {
                var sensor = await _sensorService.GetSensorTypeByid(Id);

                if (sensor == null)
                {
                    Response.StatusCode = 404;
                    return View("DashBordNotFoundErros");
                }

                await _sensorService.DeleteSensorType(sensor);

                _toastNotification.Success($"Sensor {sensor.SensorType} was successfully deleted ");
                return RedirectToAction("AddSensor");

            }
            catch (DbUpdateException)
            {
                _toastNotification.Warning("Error!! Could not delete. Please try again, and if the problem persists contact us at Data-Lab.Info.com");
                return RedirectToAction("AddSensor");
            }
        }

    }
}
