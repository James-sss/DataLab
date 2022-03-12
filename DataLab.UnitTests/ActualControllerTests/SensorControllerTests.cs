using DataLab.Controllers;
using DataLab.Models;
using DataLab.UnitTests.AbstractControllerTests;
using DataLab.ViewModels.Sensor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataLab.UnitTests.ActualControllerTests
{
    public class SensorControllerTests : AbstractSensorTestController
    {
        private static readonly SensorTypes FakeSensor_1 = new SensorTypes() { SensorTypeId = 1, SensorType = "Type 1 " };
        private static readonly SensorTypes FakeSensor_2 = new SensorTypes() { SensorTypeId = 2, SensorType = "Type 2 " };

        public SensorControllerTests() : base(new List<SensorTypes>() { FakeSensor_1, FakeSensor_2 })
        {
        }

        [Fact]
        public void AddSensorHttpGet_ShouldReturn_ViewModel()
        {
            var result = _controller.AddSensor();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData.Model);
        }


        [Fact]
        public void AddSensorHttpGet_ShouldReturnViewModelResults_WithListOfSensorTypes()
        {
            _SensorServiceStub.Setup(svc => svc.GetAllSensorTypes()).Returns(Items);

            var result = _controller.AddSensor();

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelVm = Assert.IsAssignableFrom<AddSensorTypeVM>(viewResult.ViewData.Model);
            Assert.Equal(Items.Count(), modelVm.ListSensorTypes.Count());
        }


        [Fact]
        public async Task AddSensorHttpPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {
            var modelVm = new AddSensorTypeVM();

            var result = await _controller.AddSensor(modelVm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.AddSensor), redirectResult.ActionName);
        }


        [Fact]
        public async Task AddSensorHttpPost_ShouldCallAddSensorTypeMethoOnce_IfModelStateIsValid()
        {
            var modelVm = new AddSensorTypeVM()
            { SensorType = "Type 1 " };

            await _controller.AddSensor(modelVm);

            _SensorServiceStub.Verify(x => x.AddSensorType(It.IsAny<SensorTypes>()), Times.Once);

        }


        [Fact]
        public async Task AddSensorHttpPost_ShouldCallAddSensorTypeMethodOnce_WithValidViewModelPropertiesAndValidModelState()
        {
            var sensor = FakeSensor_1;
            var modelVm = new AddSensorTypeVM() { SensorType = "Type 1 " };

            await _controller.AddSensor(modelVm);

            _SensorServiceStub.Verify(x => x.AddSensorType(It.Is<SensorTypes>(x => x.SensorType.Equals(sensor.SensorType))), Times.Once);
        }


        [Fact]
        public async Task AddSensorHttpPost_ShouldReturnAddSensorTypeViewModel_IfModelStateIsInvalid()
        {
            var modelVm = new AddSensorTypeVM();
            _controller.ModelState.AddModelError("error", "testerror");

            var result = await _controller.AddSensor(modelVm);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<AddSensorTypeVM>(viewResult.ViewData.Model);

        }


        [Fact]
        public async Task EditSensorHttpGet_ShouldReturnStatusCode404_ifSensorIdIsNull()
        {
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _controller.ControllerContext.HttpContext.Response;

            var result = await _controller.EditSensor(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task EditSensorHttpGet_ShouldBeOfType_EditSensorTypeViewModel()
        {
            _SensorServiceStub.Setup(x => x.GetSensorTypeByid(FakeSensor_1.SensorTypeId)).ReturnsAsync(FakeSensor_1);

            var result = await _controller.EditSensor(FakeSensor_1.SensorTypeId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditSensorTypeVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task EditSensorHttpPost_ShouldCallUpdateSensorTypeMethodOnce_WithValidViewModelPropertiesAndValidModelState()
        {
            var sensor = FakeSensor_2;
            _SensorServiceStub.Setup(x => x.GetSensorTypeByid(sensor.SensorTypeId)).ReturnsAsync(sensor);
            var modelVm = new EditSensorTypeVM() { SensorTypeId = 2, SensorType = "Type 2 " };

            await _controller.EditSensor(modelVm);

            _SensorServiceStub.Verify(x => x.UpdateSensorType(It.Is<SensorTypes>(x => x.SensorTypeId.Equals(sensor.SensorTypeId)
                                                                             && x.SensorType.Equals(sensor.SensorType))), Times.Once);
        }


        [Fact]
        public async Task EditSensorHttpPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {
            var sensor = FakeSensor_1;
            _SensorServiceStub.Setup(repo => repo.GetSensorTypeByid(sensor.SensorTypeId)).ReturnsAsync(sensor);
            var modelVm = new EditSensorTypeVM() { SensorTypeId = sensor.SensorTypeId, SensorType = sensor.SensorType };

            var result = await _controller.EditSensor(modelVm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(SensorController.AddSensor), redirectResult.ActionName);
        }


        [Fact]
        public async Task EditSensorHttpPost_ShouldReturnEditSensorTypeViewModel_IfModelStateIsInvalid()
        {
            var modelVm = new EditSensorTypeVM();
            _controller.ModelState.AddModelError("error", "testerror");

            var result = await _controller.EditSensor(modelVm);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditSensorTypeVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task DeleteSensorHttpGet_ShouldReturnStatusCode404_ifSensorIdIsNull()
        {
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _controller.ControllerContext.HttpContext.Response;

            var result = await _controller.DeleteSenser(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task DeleteSensorHttpPost_ShouldCallDeleteSensorTypeMethodOnce()
        {
            var sensor = FakeSensor_1;
            _SensorServiceStub.Setup(x => x.GetSensorTypeByid(sensor.SensorTypeId)).ReturnsAsync(sensor);

            var result = await _controller.DeleteSenser(sensor.SensorTypeId);

            _SensorServiceStub.Verify(x => x.DeleteSensorType(It.IsAny<SensorTypes>()), Times.Once);
        }


        [Fact]
        public async Task DeleteSensorHttpPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {
            var sensor = FakeSensor_1;
            _SensorServiceStub.Setup(x => x.GetSensorTypeByid(sensor.SensorTypeId)).ReturnsAsync(sensor);

            var result = await _controller.DeleteSenser(sensor.SensorTypeId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(SensorController.AddSensor), redirectResult.ActionName);
        }
    }
}
