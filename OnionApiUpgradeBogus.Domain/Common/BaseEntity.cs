using System;

namespace OnionApiUpgradeBogus.Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; set; }
    }
}