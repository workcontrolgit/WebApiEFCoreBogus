using AutoBogus;
using Bogus;
using OnionApiUpgradeBogus.Domain.Entities;
using System;

namespace OnionApiUpgradeBogus.Infrastructure.Shared.Mock
{
    public class CustomerSeedBogusConfig : AutoFaker<Customer>
    {
        public CustomerSeedBogusConfig()
        {
            Randomizer.Seed = new Random(8675309);
            RuleFor(m => m.Id, f => Guid.NewGuid());
            RuleFor(o => o.CompanyName, f => f.Name.JobTitle());
            RuleFor(o => o.ContactName, f => f.Commerce.Department());
            RuleFor(o => o.Phone, f => f.Name.JobDescriptor());
            RuleFor(o => o.Created, f => f.Date.Past(1));
            RuleFor(o => o.CreatedBy, f => f.Name.FullName());
            RuleFor(o => o.LastModified, f => f.Date.Recent(1));
            RuleFor(o => o.LastModifiedBy, f => f.Name.FullName());
        }
    }
}