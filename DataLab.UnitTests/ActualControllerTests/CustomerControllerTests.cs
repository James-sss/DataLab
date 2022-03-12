using DataLab.Controllers;
using DataLab.Enums;
using DataLab.Models;
using DataLab.UnitTests.AbstractTestControllers;
using DataLab.ViewModels.Customer;
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
    public class CustomerControllerTests : AbstractCustomerTestController
    {
        private static readonly Customers FakeCustomer1 = new Customers() { CustomerId = 1, CustomerName = "Item 1 name", Address = "Item 1 Adress", DataSource = Enum_DataSource.Farm };
        private static readonly Customers FakeCustomer2 = new Customers() { CustomerId = 2, CustomerName = "Item 2 name", Address = "Item 2 Adress", DataSource = Enum_DataSource.Factory };

        public CustomerControllerTests() : base(new List<Customers>() { FakeCustomer1, FakeCustomer2 })
        {
        }

        [Fact]
        public void AddCustomerHttpGet__ShouldReturn_ViewModel()
        {
            var result = _Controller.AddCustomer();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData.Model);
        }


        [Fact]
        public void AddCustomerHttpGet_ShouldReturnViewModelResults_WithListOfCustomers()
        {
            _customerServiceStub.Setup(srv => srv.GetAllCustomers()).Returns(Items);

            var result = _Controller.AddCustomer();

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelVm = Assert.IsAssignableFrom<AddCustomerVM>(viewResult.ViewData.Model);
            Assert.Equal(Items.Count(), modelVm.CustomerList.Count());
        }


        [Fact]
        public async Task AddCustomerHttpPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {
            var modelVm = new AddCustomerVM();

            var result = await _Controller.AddCustomer(modelVm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CustomersController.AddCustomer), redirectResult.ActionName);
        }


        [Fact]
        public async Task AddCustomerHttpPost_ShouldCallAddCustomerMethodOnce_IfModelStateIsValid()
        {
            var modelVm = new AddCustomerVM(){ CustomerName = FakeCustomer1.CustomerName };

            await _Controller.AddCustomer(modelVm);

            _customerServiceStub.Verify(x => x.AddCustomer(It.IsAny<Customers>()), Times.Once);

        }


        [Fact]
        public async Task AddCustomerHttpPost_ShouldCallAddCustomerMethodOnce_WithValidViewModelPropertiesAndValidModelState()
        {
            var customer = FakeCustomer1;
            var modelVm = new AddCustomerVM() { CustomerName = customer.CustomerName };

            await _Controller.AddCustomer(modelVm);

            _customerServiceStub.Verify(x => x.AddCustomer(It.Is<Customers>(x => x.CustomerName.Equals(customer.CustomerName))), Times.Once);
        }


        [Fact]
        public async Task AddCustomerHttpPost_ShouldReturnAddCustomerViewModel_IfModelStateIsInvalid()
        {
            var modelVm = new AddCustomerVM();
            _Controller.ModelState.AddModelError("error", "testerror");

            var result = await _Controller.AddCustomer(modelVm);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<AddCustomerVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task EditCustomerHttGet_ShouldReturnStatusCode404_ifSensorIdIsNull()
        {
            _Controller.ControllerContext = new ControllerContext();
            _Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _Controller.ControllerContext.HttpContext.Response;

            var result = await _Controller.EditCustomer(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task EditCustomerHttGet_ShouldBeOfType_EditCustomerViewModel()
        {
            _customerServiceStub.Setup(x => x.GetCustomerByid(FakeCustomer1.CustomerId)).ReturnsAsync(FakeCustomer1);

            var result = await _Controller.EditCustomer(FakeCustomer1.CustomerId);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditCustomerVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task EditCustomerHttPost_ShouldCallUpdateCustomerMethodOnce_WithValidViewModelPropertiesAndValidModelState()
        {
            var customer = FakeCustomer1;
            _customerServiceStub.Setup(x => x.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new EditCustomerVM() { CustomerId = customer.CustomerId, CustomerName = customer.CustomerName };

            await _Controller.EditCustomer(modelVm);

            _customerServiceStub.Verify(x => x.UpdateCustomer(It.Is<Customers>(x => x.CustomerId.Equals(customer.CustomerId)
                                                                             && x.CustomerName.Equals(customer.CustomerName))), Times.Once);
        }


        [Fact]
        public async Task EditCustomerHttPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {
            var customer = FakeCustomer1;
            _customerServiceStub.Setup(svr => svr.GetCustomerByid(customer.CustomerId)).ReturnsAsync(customer);
            var modelVm = new EditCustomerVM() { CustomerId = customer.CustomerId };

            var result = await _Controller.EditCustomer(modelVm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CustomersController.AddCustomer), redirectResult.ActionName);
        }


        [Fact]
        public async Task EditCustomerHttPost_ShouldReturnEditCustomerViewModel_IfModelStateIsInvalid()
        {
            var modelVm = new EditCustomerVM();
            _Controller.ModelState.AddModelError("error", "testerror");

            var result = await _Controller.EditCustomer(modelVm);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditCustomerVM>(viewResult.ViewData.Model);
        }


        [Fact]
        public async Task DeleteCustomerHttGet_ShouldReturnStatusCode404_ifSensorIdIsNull()
        {
            _Controller.ControllerContext = new ControllerContext();
            _Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var response = _Controller.ControllerContext.HttpContext.Response;

            var result = await _Controller.DeleteCustomer(0);

            Assert.Equal(404, response.StatusCode);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task DeleteCustomerHttPost_ShouldCallDeleteCustomerMethodOnce()
        {
            var customer = FakeCustomer1;
            _customerServiceStub.Setup(x => x.GetCustomerByid(FakeCustomer1.CustomerId)).ReturnsAsync(FakeCustomer1);

            var result = await _Controller.DeleteCustomer(customer.CustomerId);

            _customerServiceStub.Verify(x => x.DeleteCustomer(It.IsAny<Customers>()), Times.Once);
        }


        [Fact]
        public async Task DeleteCustomerHttPost_ShouldReturnARedirectToAction_AfterSuccessfulExecution()
        {
            var customer = FakeCustomer1;
            _customerServiceStub.Setup(x => x.GetCustomerByid(FakeCustomer1.CustomerId)).ReturnsAsync(FakeCustomer1);

            var result = await _Controller.DeleteCustomer(customer.CustomerId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CustomersController.AddCustomer), redirectResult.ActionName);
        }

    }
}
