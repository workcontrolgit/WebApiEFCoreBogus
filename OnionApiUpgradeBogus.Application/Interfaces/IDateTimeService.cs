using System;

namespace OnionApiUpgradeBogus.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}