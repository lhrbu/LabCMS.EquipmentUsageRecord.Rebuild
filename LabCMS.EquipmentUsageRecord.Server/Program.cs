using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using LabCMS.EquipmentUsageRecord.Server.Services;

namespace LabCMS.EquipmentUsageRecord.Server
{
    public class Program
    {
        public const string Version = "3.0.0";
        public static void Main(string[] args)
        {
            
            RegisterSyncfusion();
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        private static void RegisterSyncfusion()=>
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                "MzgxNzI3QDMxMzgyZTM0MmUzMENtVFBCVzFIenlNM2pMNWszWHR6emFyU0M4SVM3MEN6cnZoV2NjTnVQMjQ9");
    }
}
