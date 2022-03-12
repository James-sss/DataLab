using DataLab.Controllers;
using DataLab.Enums;
using DataLab.Models;
using DataLab.UnitTests.AbstractTestControllers;
using DataLab.ViewModels.Data;
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
    public class DataControllerTests : AbstractDataTestController
    {
        private static readonly CollectedData FakeData = new CollectedData() { Id = 1, CustomerId = 1, Location = "Item 1 Location", SensorTypeId = 1, Datetime = DateTime.Today, Value = 5 };
        private static readonly Customers FakeCustomer = new Customers() { CustomerId = 1, CustomerName = "Item 1 name", Address = "Item 1 Adress", DataSource = Enum_DataSource.Farm };


        [Fact]
        public async Task FetchData_ShouldReturnStatusCode404_ifIdIsNull()
        {

            _Controller.ControllerContext = new ControllerContext();
            _Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _Controller.ControllerContext.HttpContext.Response;

            var result = await _Controller.FetchData(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task FetchData_ShouldReturn_ViewModel()
        {
            var customer = FakeCustomer;
            _customerServiceStub.Setup(x => x.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new FetchDataVM();

            var result = await _Controller.FetchData(customer.CustomerId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData.Model);

        }


        [Fact]
        public async Task AddDataHttpGet_ShouldReturnStatusCode404_ifIdIsNull()
        {
            _Controller.ControllerContext = new ControllerContext();
            _Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _Controller.ControllerContext.HttpContext.Response;

            var result = await _Controller.FetchData(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task AddDataHttpGet_ShouldReturn_ViewModel()
        {
            var customer = FakeCustomer;
            _customerServiceStub.Setup(x => x.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new AddDataVM(){ CustomerId = customer.CustomerId };

            var result = await _Controller.AddData(modelVm.CustomerId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task AddDataHttpPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {
            var customer = FakeCustomer;
            _customerServiceStub.Setup(x => x.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new AddDataVM() { CustomerId = customer.CustomerId };

            var result = await _Controller.AddData(modelVm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(DataController.AddData), redirectResult.ActionName);
        }


        [Fact]
        public async Task AddDataHttpPost_ShouldCallAddDataMethoOnce_IfModelStateIsValid()
        {
            var customer = FakeCustomer;
            _customerServiceStub.Setup(x => x.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new AddDataVM() { CustomerId = customer.CustomerId };

            var result = await _Controller.AddData(modelVm);

            _dataServiceStub.Verify(x => x.AddData(It.IsAny<CollectedData>()), Times.Once);
        }


        [Fact]
        public async Task AddSensorHttpPost_ShouldCallAddDataMethodOnce_WithValidViewModelPropertiesAndValidModelState()
        {
            var customer = FakeCustomer;
            _customerServiceStub.Setup(x => x.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new AddDataVM() { CustomerId = customer.CustomerId, Location = FakeData.Location};

            var result = await _Controller.AddData(modelVm);

            _dataServiceStub.Verify(x => x.AddData(It.Is<CollectedData>(x => x.Location.Equals(modelVm.Location))), Times.Once);
        }


        [Fact]
        public async Task AddSensorHttpPost_ShouldReturnAddDataViewModel_IfModelStateIsInvalid()
        {
            var customer = FakeCustomer;
            _customerServiceStub.Setup(x => x.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new AddDataVM() { CustomerId = customer.CustomerId };
            _Controller.ModelState.AddModelError("error", "testerror");

            var result = await _Controller.AddData(modelVm.CustomerId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<AddDataVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task EditDataHttpGet_ShouldReturnStatusCode404_ifIdIsNull()
        {
            _Controller.ControllerContext = new ControllerContext();
            _Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _Controller.ControllerContext.HttpContext.Response;

            var result = await _Controller.EditData(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task EditDataHttpGet_ShouldBeOfType_EditDataViewModel()
        {
            _dataServiceStub.Setup(x => x.GetDataByid(FakeData.Id)).ReturnsAsync(FakeData);

            var result = await _Controller.EditData(FakeData.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditDataVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task EditDataHttpPost_ShouldCallUpdateDataMethodOnce_WithValidViewModelPropertiesAndValidModelState()
        {
            var data = FakeData;
            _dataServiceStub.Setup(x => x.GetDataByid(data.Id)).ReturnsAsync(data);
            var modelVm = new EditDataVM() { CollectedDataId = data.Id, Location = data.Location };

            await _Controller.EditData(modelVm);

            _dataServiceStub.Verify(x => x.UpdateData(It.Is<CollectedData>(x => x.Id.Equals(data.Id))), Times.Once);
        }


        [Fact]
        public async Task EditDataHttpPost_ShouldReturnEditDataViewModel_IfModelStateIsInvalid()
        {
            var data = FakeData;
            _dataServiceStub.Setup(x => x.GetDataByid(data.Id)).ReturnsAsync(data);
            var modelVm = new EditDataVM() { CollectedDataId = data.Id };
            _Controller.ModelState.AddModelError("error", "testerror");

            var result = await _Controller.EditData(modelVm.CollectedDataId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditDataVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task DeleteDataHttpGet_ShouldReturnStatusCode404_ifIdIsNull()
        {
            _Controller.ControllerContext = new ControllerContext();
            _Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _Controller.ControllerContext.HttpContext.Response;

            var result = await _Controller.DeleteData(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task DeleteDataHttpPost_ShouldCallDeleteSensorTypeMethodOnce()
        {
            var data = FakeData;
            _dataServiceStub.Setup(x => x.GetDataByid(data.Id)).ReturnsAsync(data);

            var result = await _Controller.DeleteData(data.Id);

            _dataServiceStub.Verify(x => x.DeleteData(It.IsAny<CollectedData>()), Times.Once);
        }


        [Fact]
        public async Task DeleteDataHttpPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {

            var data = FakeData;
            _dataServiceStub.Setup(x => x.GetDataByid(data.Id)).ReturnsAsync(data);

            var result = await _Controller.DeleteData(data.Id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(DataController.FetchData), redirectResult.ActionName);
        }
    }
}
