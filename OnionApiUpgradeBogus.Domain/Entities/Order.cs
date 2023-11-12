using OnionApiUpgradeBogus.Domain.Common;
using System;
using System.Collections.Generic;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class Order : AuditableBaseEntity
    {
        public DateTime OrderDate { get; set; }
        public string Terms { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
