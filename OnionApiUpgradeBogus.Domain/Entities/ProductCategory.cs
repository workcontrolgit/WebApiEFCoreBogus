using OnionApiUpgradeBogus.Domain.Common;
using System.Collections.Generic;

namespace OnionApiUpgradeBogus.Domain.Entities;

public class ProductCategory : AuditableBaseEntity
{
    public string Name { get; set; } = null!;
}