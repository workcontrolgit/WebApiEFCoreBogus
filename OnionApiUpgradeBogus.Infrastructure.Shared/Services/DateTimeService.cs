using OnionApiUpgradeBogus.Application.Interfaces;
using System;

namespace OnionApiUpgradeBogus.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}