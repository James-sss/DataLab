using DataLab.DataManager;
using DataLab.Migrations;
using DataLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLab.Repositories
{
    public class CustomerRepository : ICustomerService
    {
        private readonly DataLabDbContext _context;

        public CustomerRepository(DataLabDbContext context)
        {
            _context = context;
        }

        public Task<Customers> AddCustomer(Customers customer)
        {
            throw new NotImplementedException();
        }

        public Task<Customers> DeleteCustomer(Customers customer)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customers> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<Customers> GetCustomerByid(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Customers> UpdateCustomer(Customers customer)
        {
            throw new NotImplementedException();
        }
    }
}
