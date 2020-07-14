﻿using Microsoft.EntityFrameworkCore;
using NorthWindApiDemo.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthWindApiDemo.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private NorthWindContext _context;

        public CustomerRepository(NorthWindContext context) 
        {
            _context = context;
        }

        public void AddOrder(string customerId, Orders order)
        {
            var customer = GetCustomer(customerId, false);
            customer.Orders.Add(order);
        }

        public bool CustometExist(string customerId)
        {
            return _context.Customers.Any(c => c.CustomerId == customerId);
        }

        public void DeleteOrder(Orders order)
        {
            _context.Orders.Remove(order);
        }

        public Customers GetCustomer(string customerId, bool includedOrders)
        {
            if (includedOrders)
                return _context.Customers.Include(c => c.Orders).Where(c => c.CustomerId == customerId).FirstOrDefault();
            return _context.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
        }

        public IEnumerable<Customers> GetCustomers()
        {
            return _context.Customers.OrderBy(c=> c.CompanyName).ToList();
        }

        public Orders GetOrder(string customerId, int orderId)
        {
            return _context.Orders.Where(c => c.CustomerId == customerId && c.OrderId == orderId).FirstOrDefault();
        }

        public IEnumerable<Orders> GetOrders(string customerId)
        {
            return _context.Orders.Where(c => c.CustomerId == customerId).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
