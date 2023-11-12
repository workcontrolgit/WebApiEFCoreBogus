﻿using LinqKit;
using Microsoft.EntityFrameworkCore;
using OnionApiUpgradeBogus.Application.Features.Customers.Queries.GetCustomers;
using OnionApiUpgradeBogus.Application.Interfaces;
using OnionApiUpgradeBogus.Application.Interfaces.Repositories;
using OnionApiUpgradeBogus.Application.Parameters;
using OnionApiUpgradeBogus.Domain.Entities;
using OnionApiUpgradeBogus.Infrastructure.Persistence.Contexts;
using OnionApiUpgradeBogus.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Infrastructure.Persistence.Repositories
{
    public class CustomerRepositoryAsync : GenericRepositoryAsync<Customer>, ICustomerRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Customer> _customers;
        private IDataShapeHelper<Customer> _dataShaper;
        private readonly IMockService _mockData;

        public CustomerRepositoryAsync(ApplicationDbContext dbContext,
            IDataShapeHelper<Customer> dataShaper, IMockService mockData) : base(dbContext)
        {
            _dbContext = dbContext;
            _customers = dbContext.Set<Customer>();
            _dataShaper = dataShaper;
            _mockData = mockData;
        }

        public async Task<bool> IsUniqueCustomerNumberAsync(string companyName)
        {
            return await _customers
                .AllAsync(p => p.CompanyName != companyName);
        }

        public async Task<bool> SeedDataAsync(int rowCount)
        {
            foreach (Customer position in _mockData.GetCustomers(rowCount))
            {
                await this.AddAsync(position);
            }
            return true;
        }

        public async Task<(IEnumerable<Entity> data, RecordsCount recordsCount)> GetPagedCustomerReponseAsync(GetCustomersQuery requestParameter)
        {
            var companyName = requestParameter.CompanyName;
            var contactName = requestParameter.ContactName;

            var pageNumber = requestParameter.PageNumber;
            var pageSize = requestParameter.PageSize;
            var orderBy = requestParameter.OrderBy;
            var fields = requestParameter.Fields;

            int recordsTotal, recordsFiltered;

            // Setup IQueryable
            var result = _customers
                .AsNoTracking()
                .AsExpandable();

            // Count records total
            recordsTotal = await result.CountAsync();

            // filter data
            FilterByColumn(ref result, companyName, contactName);

            // Count records after filter
            recordsFiltered = await result.CountAsync();

            //set Record counts
            var recordsCount = new RecordsCount
            {
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            // set order by
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                result = result.OrderBy(orderBy);
            }

            // select columns
            if (!string.IsNullOrWhiteSpace(fields))
            {
                result = result.Select<Customer>("new(" + fields + ")");
            }
            // paging
            result = result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // retrieve data to list
            var resultData = await result.ToListAsync();
            // shape data
            var shapeData = _dataShaper.ShapeData(resultData, fields);

            return (shapeData, recordsCount);
        }

        private void FilterByColumn(ref IQueryable<Customer> query, string companyName, string contactName)
        {
            if (!query.Any())
                return;

            if (string.IsNullOrEmpty(contactName) && string.IsNullOrEmpty(companyName))
                return;

            var predicate = PredicateBuilder.New<Customer>();

            if (!string.IsNullOrEmpty(companyName))
                predicate = predicate.Or(p => p.CompanyName.Contains(companyName.Trim()));

            if (!string.IsNullOrEmpty(contactName))
                predicate = predicate.Or(p => p.ContactName.Contains(contactName.Trim()));

            query = query.Where(predicate);
        }
    }
}