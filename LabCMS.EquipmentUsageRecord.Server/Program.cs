using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Nest;
using Microsoft.Extensions.DependencyInjection;
using LabCMS.EquipmentUsageRecord.Server.Services;

namespace LabCMS.EquipmentUsageRecord.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = CreateHostBuilder(args).Build();
            //var service = host.Services.GetRequiredService<SecretEncryptService>();
            //var b64 = service.Encrypt("host=localhost:password=1234qwer");
            //var content = service.Decrypt(b64);
            CreateHostBuilder(args).Build().Run();
        }
        

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
