﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using OnionApiUpgradeBogus.Application.Interfaces;
using OnionApiUpgradeBogus.Domain.Common;
using OnionApiUpgradeBogus.Domain.Entities;
using OnionApiUpgradeBogus.Infrastructure.Shared.Services;
using System.Threading;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly ILoggerFactory _loggerFactory;

        private readonly Fakers _fakers;

        private readonly IMockService _mockService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IDateTimeService dateTime,
            Fakers fakers,
            IMockService mockService,
            ILoggerFactory loggerFactory
            ) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _loggerFactory = loggerFactory;
            _fakers = fakers;
            _mockService = mockService;
        }

        //public DbSet<Position> Positions { get; set; }

        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }



        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<ProductProductCategory> ProductProductCategories => Set<ProductProductCategory>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var _mockData = this.Database.GetService<IMockService>();
            var seedPositions = _mockData.SeedPositions(1000);

            var _fakers = this.Database.GetService<Fakers>();

            //var _mockData = this.Database.GetService<IMockService>();

            //if (_mockService != null)
            //{
            //    var seedPositions2 = _mockService.SeedPositions(1000);
            //}
            //if (_fakers != null)
            //{
            //    var customers2 = _fakers.GetCustomerGenerator(false).Generate(50);
            //}


            modelBuilder.Entity<Position>().HasData(seedPositions);

            var addresses = _fakers.GetAddressGenerator().Generate(50);

            modelBuilder.Entity<Address>().HasData(addresses);

            var customers = _fakers.GetCustomerGenerator(false).Generate(50);

            for (var x = 0; x < customers.Count; ++x)
            {
                customers[x].AddressId = addresses[x].Id;
            }

            modelBuilder.Entity<Customer>()
                .HasData(customers);

            // Configure the tables
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());

            // Generate seed data with Bogus
            var databaseSeeder = new DatabaseSeeder();

            // Apply the seed data on the tables
            modelBuilder.Entity<Product>().HasData(databaseSeeder.Products);
            modelBuilder.Entity<ProductCategory>().HasData(databaseSeeder.ProductCategories);
            modelBuilder.Entity<ProductProductCategory>().HasData(databaseSeeder.ProductProductCategories);


            base.OnModelCreating(modelBuilder);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }

    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.CreationDate).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }

    internal class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategory");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
        }
    }

    internal class ProductProductCategoryConfiguration : IEntityTypeConfiguration<ProductProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductProductCategory> builder)
        {
            builder.ToTable("ProductProductCategory");

            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder.HasOne(x => x.Product)
                .WithMany(x => x.ProductProductCategories)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
        }
    }
}