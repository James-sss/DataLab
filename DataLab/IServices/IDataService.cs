using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.IServices
{
    public interface IDataService
    {
        Task<CollectedData> GetDataByid(int Id);
        IEnumerable<CollectedData> GetAllData();
        Task<CollectedData> AddData(CollectedData data);
        Task<CollectedData> UpdateData(CollectedData data);
        Task<CollectedData> DeleteData(CollectedData data);

        IEnumerable<CollectedData> GetAllDataByCustId(int customerid);
    }
}
