using OnionApiUpgradeBogus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class OrderItem : AuditableBaseEntity
    {
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
