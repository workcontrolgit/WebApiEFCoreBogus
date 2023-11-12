using System;

namespace OnionApiUpgradeBogus.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersViewModel
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
    }
}