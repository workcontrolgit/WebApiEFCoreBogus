using Bogus;
using OnionApiUpgradeBogus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Infrastructure.Shared.Services
{
    public class Fakers
    {

        Faker<Customer>? _customerFaker = null;
        Faker<Address>? _addressFaker = null;

        public Faker<Customer> GetCustomerGenerator(bool includeAddresses = true)
        {
            if (_customerFaker is null)
            {
                var addressFaker = GetAddressGenerator();
                var id = 0;
                _customerFaker = new Faker<Customer>()
                  .UseSeed(1969)
                  .RuleFor(c => c.Id, f => Guid.NewGuid())
                  .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                  .RuleFor(c => c.ContactName, f => f.Name.FullName())
                  .RuleFor(c => c.Phone, f => f.Phone.PhoneNumberFormat().OrNull(f, .15f));

                if (includeAddresses)
                {
                    _customerFaker = _customerFaker
                      .RuleFor(c => c.Address, f => addressFaker.Generate(1).First().OrNull(f, .1f));
                }

            }

            return _customerFaker;
        }

        public Faker<Address> GetAddressGenerator()
        {
            if (_addressFaker is null)
            {
                _addressFaker = new Faker<Address>()
                  .UseSeed(1969)
                  .RuleFor(c => c.Id, f => Guid.NewGuid())
                  .RuleFor(c => c.Address1, f => f.Address.StreetAddress())
                  .RuleFor(c => c.Address2, f => f.Address.SecondaryAddress().OrNull(f, .5f))
                  .RuleFor(c => c.City, f => f.Address.City())
                  .RuleFor(c => c.StateProvince, f => f.Address.State())
                  .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
                  ;

            }

            return _addressFaker;
        }

    }
}
