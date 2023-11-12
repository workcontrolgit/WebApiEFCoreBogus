using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionApiUpgradeBogus.Application.Interfaces;
using OnionApiUpgradeBogus.Domain.Settings;
using OnionApiUpgradeBogus.Infrastructure.Shared.Services;

namespace OnionApiUpgradeBogus.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IMockService, MockService>();
            // services.AddTransient<Fakers>();

        }
    }
}