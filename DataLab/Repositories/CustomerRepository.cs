using DataLab.DataManager;
using DataLab.IServices;
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

        public async Task<Customers> AddCustomer(Customers customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customers> DeleteCustomer(Customers customer)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public IEnumerable<Customers> GetAllCustomers()
        {
            return _context.Customers;
        }

        public async Task<Customers> GetCustomerByid(int Id)
        {
            return await _context.Customers.FindAsync(Id);
        }

        public async Task<Customers> UpdateCustomer(Customers customer)
        {
            var Object = _context.Customers.Attach(customer);
            Object.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
