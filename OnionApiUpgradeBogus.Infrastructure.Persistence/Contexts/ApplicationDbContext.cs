using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

        public DbSet<Position> Positions { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        //public DbSet<Customer> Customers => Set<Customer>();
        //public DbSet<Address> Addresses => Set<Address>();
        //public DbSet<Order> Orders => Set<Order>();
        //public DbSet<OrderItem> OrderItems => Set<OrderItem>();

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
            base.OnModelCreating(modelBuilder);


            var _mockData = this.Database.GetService<IMockService>();
            var seedPositions = _mockData.SeedPositions(1000);

            var _fakers2 = this.Database.GetService<Fakers>();

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

            var addresses = _fakers2.GetAddressGenerator().Generate(50);

            modelBuilder.Entity<Address>().HasData(addresses);

            var customers = _fakers2.GetCustomerGenerator(false).Generate(50);

            for (var x = 0; x < customers.Count; ++x)
            {
                customers[x].AddressId = addresses[x].Id;
            }

            modelBuilder.Entity<Customer>()
                .HasData(customers);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}