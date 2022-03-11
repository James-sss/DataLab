using AspNetCoreHero.ToastNotification.Abstractions;
using DataLab.Controllers;
using DataLab.IServices;
using DataLab.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLab.UnitTests.AbstractTestControllers
{
    public class AbstractDataTestController
    {
        protected readonly DataController _Controller;
        protected readonly Mock<IDataService> _dataServiceStub = new();
        protected readonly Mock<INotyfService> _toastNotificationStub = new();
        protected readonly Mock<ICustomerService> _customerServiceStub = new();
        protected readonly Mock<ISensorService> _sensorServiceStub = new();

        protected AbstractDataTestController()
        {
           _Controller = new DataController(_dataServiceStub.Object, _toastNotificationStub.Object, _customerServiceStub.Object, _sensorServiceStub.Object);
        }
    }
}
