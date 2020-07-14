using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NorthWindApiDemo.EFModels;
using NorthWindApiDemo.Models;
using NorthWindApiDemo.Services;

namespace NorthWindApiDemo.Controllers
{
    [Route("api/customers")]
    public class OrdersController : Controller
    {
        private ICustomerRepository _customerRepository;
        private IMapper _mapper;

        public OrdersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet("{customerId}/orders")]
        public IActionResult GetOrders(string customerId)
        {
            if (!_customerRepository.CustometExist(customerId)) return NotFound();

            var orders = _customerRepository.GetOrders(customerId).ToList();

            var ordersResult = _mapper.Map<IEnumerable<OrdersDTO>>(orders); 

            return Ok(ordersResult);
        }
        [HttpGet("{customerId}/orders/{id}",Name = "GetOrder")]
        public IActionResult GetOrder(string customerId, int id)
        {
            if (!_customerRepository.CustometExist(customerId)) return NotFound();

            var order = _customerRepository.GetOrder(customerId, id);
            if (order == null) return NotFound();
            var orderResult = _mapper.Map<OrdersDTO>(order);
            return Ok(orderResult);
        }

        [HttpPost("{customerId}/orders")]
        public IActionResult CreateOrder(string customerId, [FromBody] OrdersForCreationDTO order)
        {
            if (order == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_customerRepository.CustometExist(customerId)) return NotFound();
            var finalOrder = _mapper.Map<Orders>(order);

            _customerRepository.AddOrder(customerId, finalOrder);

            if (!_customerRepository.Save()) return StatusCode(500, "Plase verify your data");

            var customerCreated = _mapper.Map<OrdersDTO>(finalOrder);

            return CreatedAtRoute("GetOrder", 
                                    new 
                                    {
                                        customerId = customerId,
                                        id = customerCreated.OrderId 
                                    }, customerCreated);
        }


        [HttpPut("{customerId}/orders/{id}")]
        public IActionResult UpdateOrder(string customerId, int id, [FromBody] OrdersForUpdateDTO order) 
        {
            if (order == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_customerRepository.CustometExist(customerId)) return NotFound();

            var existingOrder = _customerRepository.GetOrder(customerId, id);
            if (existingOrder == null) return NotFound();

            _mapper.Map(order, existingOrder);
            if (!_customerRepository.Save()) return StatusCode(500, "Please verify your data");
            return NoContent();
        }

        [HttpPatch("{customerId}/orders/{id}")]
        public IActionResult UpdateOrder(string customerId, int id, 
                        [FromBody] JsonPatchDocument<OrdersForUpdateDTO> patchDocument) 
        {
            if (patchDocument == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_customerRepository.CustometExist(customerId)) return NotFound();
            var existingOrder = _customerRepository.GetOrder(customerId, id);
            if (existingOrder == null) return NotFound();

            var orderToUpdate = _mapper.Map<OrdersForUpdateDTO>(existingOrder);

            patchDocument.ApplyTo(orderToUpdate, ModelState);

            TryValidateModel(orderToUpdate);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(orderToUpdate, existingOrder);

            if (!_customerRepository.Save()) return StatusCode(500, "Please verify your data");

            return NoContent();
        }

        [HttpDelete("{customerId}/orders/{id}")]
        public IActionResult DeleteOrder(string customerId, int id)
        {
            if (!_customerRepository.CustometExist(customerId)) return NotFound();
            var existingOrder = _customerRepository.GetOrder(customerId, id);
            if (existingOrder == null) return NotFound();

            _customerRepository.DeleteOrder(existingOrder);

            if (!_customerRepository.Save()) return StatusCode(500, "Please verify your data");
            
            return NoContent();
        }
    }
}
