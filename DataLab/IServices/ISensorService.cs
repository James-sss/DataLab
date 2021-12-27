using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.IServices
{
    public interface ISensorService
    {
        Task<SensorTypes> GetSensorTypeByid(int Id);
        IEnumerable<SensorTypes> GetAllSensorTypes();
        Task<SensorTypes> AddSensorType(SensorTypes sensor);
        Task<SensorTypes> UpdateSensorType(SensorTypes sensor);
        Task<SensorTypes> DeleteSensorType(SensorTypes sensor);
    }
}
