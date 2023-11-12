using OnionApiUpgradeBogus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class Customer : AuditableBaseEntity
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public Guid AddressId { get; set; }
        public Address? Address { get; set; }
        public IList<Order> Orders { get; set; }
    }
}
