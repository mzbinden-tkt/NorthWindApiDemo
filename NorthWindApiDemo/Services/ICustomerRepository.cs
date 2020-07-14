using NorthWindApiDemo.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthWindApiDemo.Services
{
    public interface ICustomerRepository
    {
        IEnumerable<Customers> GetCustomers();
        Customers GetCustomer(string customerId, bool includedOrders);
        IEnumerable<Orders> GetOrders(string customerId);
        Orders GetOrder(string customerId, int orderId);
        bool CustometExist(string customerId);
        void AddOrder(string customerId, Orders order);
        bool Save();
        void DeleteOrder(Orders order);
    }
}
