using LabCMS.EquipmentUsageRecord.Shared.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using EasyNetQ;

namespace LabCMS.EquipmentUsageRecord.Shared.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddUsageRecordsRepository(this IServiceCollection services,
            IConfiguration configuration)
{
            services.AddDbContextPool<UsageRecordsRepository>((serviceProvider, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(UsageRecordsRepository)));
                options.UseSnakeCaseNamingConvention();

                //options.LogTo(serviceProvider.GetRequiredService<DbLogHandleService>().EnqueueDbLog,
                //    LogLevel.Information).EnableSensitiveDataLogging();
            }, 256);
            return services;
        }

        public static IServiceCollection AddEasyNetQ(this IServiceCollection services)
        {
            services.AddSingleton<IBus>(RabbitHutch.CreateBus("host=localhost;username=lhrbu;password=112358LHR/;publisherConfirms=true;timeout=10"));
            return services;
        }
    }
}
