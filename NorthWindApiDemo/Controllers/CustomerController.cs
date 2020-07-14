using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthWindApiDemo.EFModels;
using NorthWindApiDemo.Models;
using NorthWindApiDemo.Services;

namespace NorthWindApiDemo.Controllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly IMapper _mapper;

        private ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository, IMapper mapper) 
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            var results = _mapper.Map<IEnumerable<CustomerWithOutOders>>(customers);
            return Ok(results);
        }
         
        [HttpGet("{id}")]
        public IActionResult GetCustomer(string id, bool includeOrders = false)
        {
            var customer = _customerRepository.GetCustomer(id, includeOrders);
            if (customer == null) return NotFound();
            if(includeOrders)
            {
                var customerResult = _mapper.Map<CustomerDTO>(customer);
                return Ok(customerResult);
            }
            var customerResultOnly = _mapper.Map<CustomerWithOutOders>(customer);

            return Ok(customerResultOnly);
        }
    }
}
