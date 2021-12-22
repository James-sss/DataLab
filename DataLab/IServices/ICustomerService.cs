using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.IServices
{
    public interface ICustomerService
    {
        Task<Customers> GetCustomerByid(int Id);
        IEnumerable<Customers> GetAllCustomers();
        Task<Customers> AddCustomer(Customers customer);
        Task<Customers> UpdateCustomer(Customers customer);
        Task<Customers> DeleteCustomer(Customers customer);
    }
}
