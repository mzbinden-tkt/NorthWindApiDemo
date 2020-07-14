using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NorthWindApiDemo.Controllers;
using NorthWindApiDemo.EFModels;
using NorthWindApiDemo.Models;
using NorthWindApiDemo.Services;

namespace NorthWindApiDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting= false);
            services.AddDbContext<NorthWindContext>(option => {
                option.UseSqlServer("Server=(LocalDb)\\NorthWinds;Database=NorthWind;Trusted_Connection=True");
            });
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddAutoMapper(typeof(NorthWindProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            
            //var configMapper = new MapperConfiguration(
            //    cp => 
            //    {
            //        cp.CreateMap<Customers, CustomerDTO>();
            //    }).CreateMapper();

            app.UseMvc();
        }
    }
}
