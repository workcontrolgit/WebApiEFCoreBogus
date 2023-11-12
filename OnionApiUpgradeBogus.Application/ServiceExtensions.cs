﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnionApiUpgradeBogus.Application.Behaviours;
using OnionApiUpgradeBogus.Application.Helpers;
using OnionApiUpgradeBogus.Application.Interfaces;
using OnionApiUpgradeBogus.Domain.Entities;
using System.Reflection;

namespace OnionApiUpgradeBogus.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IDataShapeHelper<Position>, DataShapeHelper<Position>>();
            services.AddScoped<IDataShapeHelper<Employee>, DataShapeHelper<Employee>>();
            services.AddScoped<IDataShapeHelper<Customer>, DataShapeHelper<Customer>>();

            services.AddScoped<IModelHelper, ModelHelper>();
            // services.AddScoped<IMockService, MockService>();
        }
    }
}