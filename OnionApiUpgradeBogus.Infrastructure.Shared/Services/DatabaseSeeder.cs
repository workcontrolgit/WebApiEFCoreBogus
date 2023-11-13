using AutoBogus;
using Bogus;
using Bogus.DataSets;
using OnionApiUpgradeBogus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Address = OnionApiUpgradeBogus.Domain.Entities.Address;

namespace OnionApiUpgradeBogus.Infrastructure.Shared.Services
{
    public class DatabaseSeeder
    {
        public IReadOnlyCollection<Address> Addresses { get; } = new List<Address>();
        public IReadOnlyCollection<Customer> Customers { get; } = new List<Customer>();

        public IReadOnlyCollection<Order> Orders { get; } = new List<Order>();

        public IReadOnlyCollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

        public IReadOnlyCollection<Product> Products { get; } = new List<Product>();
        public IReadOnlyCollection<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();
        public IReadOnlyCollection<ProductProductCategory> ProductProductCategories { get; } = new List<ProductProductCategory>();

        public DatabaseSeeder()
        {
            Addresses = GenerateAddresses(amount: 1000);
            Customers = GenerateCustomers(amount: 1000, Addresses);

            Orders = GenerateOrders(amount: 1000, Customers);
            OrderItems = GenerateOrderItems(amount: 1000, Orders);


            Products = GenerateProducts(amount: 1000);
            ProductCategories = GenerateProductCategories(amount: 50);
            ProductProductCategories = GenerateProductProductCategories(amount: 1000, Products, ProductCategories);
        }

        private static IReadOnlyCollection<Order> GenerateOrders(int amount, IEnumerable<Customer> customers)
        {
            return new AutoFaker<Order>()
                .RuleFor(fake => fake.CustomerId, fake => fake.PickRandom(customers).Id)
                .Generate(amount);

            //return AutoFaker.Generate<Order>(amount);
        }

        private static IReadOnlyCollection<OrderItem> GenerateOrderItems(int amount, IEnumerable<Order> orders)
        {
            return new AutoFaker<OrderItem>()
                .RuleFor(fake => fake.OrderId, fake => fake.PickRandom(orders).Id)
                .Generate(amount);

        }


        private static IReadOnlyCollection<Address> GenerateAddresses(int amount)
        {
            var faker = new Faker<Address>()
                  .RuleFor(c => c.Id, f => Guid.NewGuid())
                  .RuleFor(c => c.Address1, f => f.Address.StreetAddress())
                  .RuleFor(c => c.Address2, f => f.Address.SecondaryAddress().OrNull(f, .5f))
                  .RuleFor(c => c.City, f => f.Address.City())
                  .RuleFor(c => c.StateProvince, f => f.Address.State())
                  .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
                  .RuleFor(c => c.Country, f => f.Address.Country())
                  .RuleFor(c => c.Created, f => f.Date.Recent())
                  .RuleFor(c => c.CreatedBy, f => f.Internet.UserName())
                  ;

            return faker.Generate(amount);

        }


        private static IReadOnlyCollection<Customer> GenerateCustomers(int amount, IEnumerable<Address> addresses)
        {
            var faker = new Faker<Customer>()
                  .RuleFor(c => c.Id, f => Guid.NewGuid())
                  .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                  .RuleFor(c => c.ContactName, f => f.Name.FullName())
                  .RuleFor(c => c.AddressId, f => f.PickRandom(addresses).Id)
                  .RuleFor(c => c.Phone, f => f.Phone.PhoneNumberFormat().OrNull(f, .15f))
                  .RuleFor(c => c.Created, f => f.Date.Recent())
                  .RuleFor(c => c.CreatedBy, f => f.Internet.UserName())
                  ;

            return faker.Generate(amount);

        }


        private static IReadOnlyCollection<Product> GenerateProducts(int amount)
        {
            var faker = new Faker<Product>()
                .RuleFor(x => x.Id, f => Guid.NewGuid()) // Each product will have an incrementing id.
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                // The refDate is very important! Without it, it will generate a random date based on the CURRENT date on your system.
                // Generating a date based on the system date is not deterministic!
                // So the solution is to pass in a constant date instead which will be used to generate a random date
                .RuleFor(x => x.CreationDate, f => f.Date.FutureOffset(
                    refDate: new DateTimeOffset(2023, 1, 16, 15, 15, 0, TimeSpan.FromHours(1))))
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                  .RuleFor(c => c.Created, f => f.Date.Recent())
                  .RuleFor(c => c.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(amount);
        }

        private static IReadOnlyCollection<ProductCategory> GenerateProductCategories(int amount)
        {
            var faker = new Faker<ProductCategory>()
                .RuleFor(x => x.Id, f => Guid.NewGuid()) // Each category will have an incrementing id.
                .RuleFor(x => x.Name, f => f.Commerce.Categories(1).First())
                  .RuleFor(c => c.Created, f => f.Date.Recent())
                  .RuleFor(c => c.CreatedBy, f => f.Internet.UserName())
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
                .RuleFor(x => x.ProductId, f => f.PickRandom(products).Id)
                .RuleFor(x => x.CategoryId, f => f.PickRandom(productCategories).Id);

            return faker.Generate(amount)
                .GroupBy(x => new { x.ProductId, x.CategoryId })
                .Select(x => x.First())
                .ToList();


        }

        private static T SeedRow<T>(Faker<T> faker, int rowId) where T : class
        {
            var recordRow = faker.UseSeed(rowId).Generate();
            return recordRow;
        }
    }
}
