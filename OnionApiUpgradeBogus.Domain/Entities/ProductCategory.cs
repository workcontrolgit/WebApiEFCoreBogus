using OnionApiUpgradeBogus.Domain.Common;

namespace OnionApiUpgradeBogus.Domain.Entities;

public class ProductCategory : AuditableBaseEntity
{
    public string Name { get; set; } = null!;
}