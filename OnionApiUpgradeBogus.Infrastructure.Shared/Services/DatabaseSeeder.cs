﻿using AutoBogus;
using Bogus;
using Bogus.DataSets;
using OnionApiUpgradeBogus.Domain.Entities;
using OnionApiUpgradeBogus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Address = OnionApiUpgradeBogus.Domain.Entities.Address;

namespace OnionApiUpgradeBogus.Infrastructure.Shared.Services
{
    public class DatabaseSeeder
    {
        public IReadOnlyCollection<Product> Products { get; } = new List<Product>();
        public IReadOnlyCollection<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();
        public IReadOnlyCollection<ProductProductCategory> ProductProductCategories { get; } = new List<ProductProductCategory>();

        public IReadOnlyCollection<Address> Addresses { get; } = new List<Address>();
        public IReadOnlyCollection<Customer> Customers { get; } = new List<Customer>();

        public IReadOnlyCollection<Order> Orders { get; } = new List<Order>();

        public IReadOnlyCollection<OrderItem> OrderItems { get; } = new List<OrderItem>();


        public DatabaseSeeder()
        {
            Products = GenerateProducts(amount: 100);
            ProductCategories = GenerateProductCategories(amount: 100);
            ProductProductCategories = GenerateProductProductCategories(amount: 100, Products, ProductCategories);

            Addresses = GenerateAddresses(amount: 100);
            Customers = GenerateCustomers(amount: 100, Addresses);

            Orders = GenerateOrders(amount: 100, Customers);
            OrderItems = GenerateOrderItems(amount: 100, Orders, Products);


        }

        private static IReadOnlyCollection<Order> GenerateOrders(int amount, IEnumerable<Customer> customers)
        {
            var faker = new Faker<Order>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.OrderDate, f => f.Date.Recent())
                .RuleFor(r => r.Terms, f => f.PickRandom<Terms>())
                .RuleFor(r => r.CustomerId, f => f.PickRandom(customers).Id)
                .RuleFor(r => r.Created, f => f.Date.Recent())
                .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(amount);
        }

        private static IReadOnlyCollection<OrderItem> GenerateOrderItems(int amount, IEnumerable<Order> orders, IEnumerable<Product> products)
        {
            var faker = new Faker<OrderItem>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.OrderId, f => f.PickRandom(orders).Id)
                .RuleFor(r => r.ProductId, f => f.PickRandom(products).Id)
                .RuleFor(r => r.UnitPrice, f => f.Commerce.Price(1).First())
                .RuleFor(r => r.Quantity, f => f.Random.Number(1, 100))
                .RuleFor(r => r.Discount, f => f.Random.Number(1, 100))
                .RuleFor(r => r.Created, f => f.Date.Recent())
                .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(amount);

        }


        private static IReadOnlyCollection<Address> GenerateAddresses(int amount)
        {
            var faker = new Faker<Address>()
                  .RuleFor(r => r.Id, f => Guid.NewGuid())
                  .RuleFor(r => r.Address1, f => f.Address.StreetAddress())
                  .RuleFor(r => r.Address2, f => f.Address.SecondaryAddress().OrNull(f, .5f))
                  .RuleFor(r => r.City, f => f.Address.City())
                  .RuleFor(r => r.StateProvince, f => f.Address.State())
                  .RuleFor(r => r.PostalCode, f => f.Address.ZipCode())
                  .RuleFor(r => r.Country, f => f.Address.Country())
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                  ;

            return faker.Generate(amount);

        }


        private static IReadOnlyCollection<Customer> GenerateCustomers(int amount, IEnumerable<Address> addresses)
        {
            var faker = new Faker<Customer>()
                  .RuleFor(r => r.Id, f => Guid.NewGuid())
                  .RuleFor(r => r.CompanyName, f => f.Company.CompanyName())
                  .RuleFor(r => r.ContactName, f => f.Name.FullName())
                  .RuleFor(r => r.AddressId, f => f.PickRandom(addresses).Id)
                  .RuleFor(r => r.Phone, f => f.Phone.PhoneNumberFormat().OrNull(f, .15f))
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                  ;

            return faker.Generate(amount);

        }


        private static IReadOnlyCollection<Product> GenerateProducts(int amount)
        {
            var faker = new Faker<Product>()
                .RuleFor(r => r.Id, f => Guid.NewGuid()) // Each product will have an incrementing id.
                .RuleFor(r => r.Name, f => f.Commerce.ProductName())
                // The refDate is very important! Without it, it will generate a random date based on the CURRENT date on your system.
                // Generating a date based on the system date is not deterministic!
                // So the solution is to pass in a constant date instead which will be used to generate a random date
                .RuleFor(r => r.CreationDate, f => f.Date.FutureOffset(
                    refDate: new DateTimeOffset(2023, 1, 16, 15, 15, 0, TimeSpan.FromHours(1))))
                .RuleFor(r => r.Description, f => f.Commerce.ProductDescription())
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(amount);
        }

        private static IReadOnlyCollection<ProductCategory> GenerateProductCategories(int amount)
        {
            var faker = new Faker<ProductCategory>()
                .RuleFor(r => r.Id, f => Guid.NewGuid()) // Each category will have an incrementing id.
                .RuleFor(r => r.Name, f => f.Commerce.Categories(1).First())
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(amount);
        }

        private static IReadOnlyCollection<ProductProductCategory> GenerateProductProductCategories(
            int amount,
            IEnumerable<Product> products,
            IEnumerable<ProductCategory> productCategories)
        {
            // Now we set up the faker for our join table.
            // We do this by grabbing a random product and category that were generated.
            var faker = new Faker<ProductProductCategory>()
                .RuleFor(r => r.ProductId, f => f.PickRandom(products).Id)
                .RuleFor(r => r.CategoryId, f => f.PickRandom(productCategories).Id);

            return faker.Generate(amount)
                .GroupBy(r => new { r.ProductId, r.CategoryId })
                .Select(r => r.First())
                .ToList();


        }

        private static T SeedRow<T>(Faker<T> faker, int rowId) where T : class
        {
            var recordRow = faker.UseSeed(rowId).Generate();
            return recordRow;
        }
    }
}
