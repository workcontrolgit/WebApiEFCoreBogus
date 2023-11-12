using OnionApiUpgradeBogus.Domain.Common;
using System;

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
