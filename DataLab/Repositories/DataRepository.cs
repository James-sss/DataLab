using DataLab.DataManager;
using DataLab.IServices;
using DataLab.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Repositories
{
    public class DataRepository : IDataService
    {
        private readonly DataLabDbContext _context;

        public DataRepository(DataLabDbContext context)
        {
            _context = context;
        }

        public async Task<CollectedData> AddData(CollectedData data)
        {
            await _context.CollectedData.AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<CollectedData> DeleteData(CollectedData data)
        {
            _context.CollectedData.Remove(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public IEnumerable<CollectedData> GetAllData()
        {
            return _context.CollectedData;
        }

        public IEnumerable<CollectedData> GetAllDataByCustId(int customerid)
        {
            return _context.CollectedData.Where(e => e.Customers.CustomerId == customerid).Include(e => e.SensorTypes).ToList();
        }

        public Task<CollectedData> GetDataByid(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<CollectedData> UpdateData(CollectedData data)
        {
            var Object = _context.CollectedData.Attach(data);
            Object.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return data;
        }
    }
}
