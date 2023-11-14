using OnionApiUpgradeBogus.Domain.Common;
using OnionApiUpgradeBogus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class Order : AuditableBaseEntity
    {
        public DateTime OrderDate { get; set; }
        public Terms Terms { get; set; }
        public Guid CustomerId { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
    }
}
