using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.Controllers;
using DataLab.IServices;
using DataLab.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLab.UnitTests.AbstractTestControllers
{
    public class AbstractCustomerTestController
    {
        protected readonly List<Customers> Items;
        protected readonly CustomersController _Controller;
        protected readonly Mock<ICustomerService> _customerServiceStub = new();
        protected readonly Mock<INotyfService> _toastNotificationStub = new();
        protected readonly Mock<IAuthUserService> _authUserServiceStub = new();
        protected readonly Mock<UserManager<ApplicationUser>> _userManagerStub = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        protected AbstractCustomerTestController(List<Customers> items)
        {
            Items = items;
            _customerServiceStub.Setup(x => x.GetAllCustomers())
                .Returns(Items);
            _Controller = new CustomersController(_customerServiceStub.Object, _toastNotificationStub.Object, _authUserServiceStub.Object, _userManagerStub.Object);
        }
    }
}
