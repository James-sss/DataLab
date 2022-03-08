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


namespace DataLab.UnitTests.AbstractControllerTests
{
    public class AbstractSensorTestController
    {
        protected readonly List<SensorTypes> Items;
        protected readonly SensorController _controller;
        protected readonly Mock<ISensorService> _SensorServiceStub = new();
        protected readonly Mock<INotyfService> _NotyfServiceStub = new();

        protected AbstractSensorTestController(List<SensorTypes> items)
        {
            Items = items;
            _SensorServiceStub.Setup(x => x.GetAllSensorTypes())
                .Returns(Items);
            _controller = new SensorController(_SensorServiceStub.Object, _NotyfServiceStub.Object);
        }
    }
}
