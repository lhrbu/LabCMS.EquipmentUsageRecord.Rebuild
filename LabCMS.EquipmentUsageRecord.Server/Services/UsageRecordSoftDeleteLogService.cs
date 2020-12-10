using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class UsageRecordSoftDeleteLogService
    {
        public UsageRecordSoftDeleteLogService(IConfiguration configuration)
        {
            Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(configuration["SoftDeleteLog"] ?? "SoftDeleteLog.log")
                .CreateLogger();
        }

        public Logger Logger {get;}
    }
}
