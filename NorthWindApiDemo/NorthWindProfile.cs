using AutoMapper;
using NorthWindApiDemo.EFModels;
using NorthWindApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthWindApiDemo
{
    public class NorthWindProfile : Profile
    {
        public NorthWindProfile() 
        {
            CreateMap<Customers, CustomerWithOutOders>();
            CreateMap<Customers, CustomerDTO>();
            CreateMap<Orders, OrdersDTO>();
            CreateMap<OrdersForCreationDTO, Orders>();
            CreateMap<OrdersForUpdateDTO, Orders>();

        }
    }
}
