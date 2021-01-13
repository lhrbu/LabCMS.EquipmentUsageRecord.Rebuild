using LabCMS.EquipmentUsageRecord.Server.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.Devkits.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Proxies
{
    public class ElasticSearchInteropProxy : DynamicProxyBase<IElasticSearchInteropService>
    {
        public bool EnableElasticSearch { get; set; } = false;
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (ServiceProvider.GetRequiredService<IConfiguration>()
                .GetValue<bool>("EnableElasticSearch"))
            {
                return targetMethod?.Invoke(Target, args);
            }
            else { return ValueTask.CompletedTask; }
        }
    }
}
