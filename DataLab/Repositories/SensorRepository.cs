using DataLab.DataManager;
using DataLab.IServices;
using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Repositories
{
    public class SensorRepository : ISensorService
    {
        private readonly DataLabDbContext _context;

        public SensorRepository(DataLabDbContext context)
        {
            _context = context;
        }

        public async Task<SensorTypes> AddSensorType(SensorTypes sensor)
        {
            await _context.SensorTypes.AddAsync(sensor);
            await _context.SaveChangesAsync();
            return sensor;
        }

        public async Task<SensorTypes> DeleteSensorType(SensorTypes sensor)
        {
            _context.SensorTypes.Remove(sensor);
            await _context.SaveChangesAsync();
            return sensor;
        }

        public IEnumerable<SensorTypes> GetAllSensorTypes()
        {
            return _context.SensorTypes;
        }

        public async Task<SensorTypes> GetSensorTypeByid(int Id)
        {
            return await _context.SensorTypes.FindAsync(Id);
        }

        public async Task<SensorTypes> UpdateSensorType(SensorTypes sensor)
        {
            var Object = _context.SensorTypes.Attach(sensor);
            Object.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return sensor;
        }
    }
}
