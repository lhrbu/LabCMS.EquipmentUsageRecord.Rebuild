using EasyNetQ;
using LabCMS.EquipmentUsageRecord.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.Extensions
{
    public static class AddEasyNetQExtensions
    {
        public static IServiceCollection AddEasyNetQ(this IServiceCollection services,IConfiguration configuration)
        {
            bool enableMQ = configuration.GetValue<bool?>("EnableMQ") ?? false;
            if (enableMQ) { services.AddSingleton<IBus>(RabbitHutch.CreateBus("host=localhost;username=lhrbu;password=112358LHR/;publisherConfirms=true;timeout=10")); }
            else { services.AddSingleton<IBus>(new DumyBus()); }
            return services;
        }
    }
}
