using LabCMS.EquipmentUsageRecord.Server.Proxies;
using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Server.Services;
using LabCMS.Gateway.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Bulkhead;
using Polly.Registry;
using Raccoon.Devkits.DynamicProxy;
using Raccoon.Devkits.JwtAuthorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(options=>options.JsonSerializerOptions.PropertyNamingPolicy=null);
            services.AddSwaggerGen(c =>c.SwaggerDoc("v1", new OpenApiInfo { Title = "LabCMS.EquipmentUsageRecord.Server", Version = "v1" }));
            
            services.AddSingleton<UsageRecordSoftDeleteLogService>();
            services.AddDbContextPool<UsageRecordsRepository>((serviceProvider,options) =>
            {
                options.UseNpgsql(Configuration.GetConnectionString(nameof(UsageRecordsRepository)));
                options.UseSnakeCaseNamingConvention();
                
                options.LogTo(serviceProvider.GetRequiredService<DbLogHandleService>().EnqueueDbLog, 
                    LogLevel.Information).EnableSensitiveDataLogging();
            },256);

            //services.AddSingleton<ElasticSearchInteropService>();
            services.AddSingletonProxy<IElasticSearchInteropService,
                ElasticSearchInteropService, ElasticSearchInteropProxy>();
            services.AddSingleton<DbLogHandleService>();
            services.AddBulkheadRetryAsyncFilter();
            services.AddTransient<SecretEncryptService>();
            services.AddTransient<ExcelExportService>();
            services.AddTransient<DynamicQueryService>();
            services.AddJwtAuthorization();
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,DbLogHandleService dbLogHandleService)
        {
            dbLogHandleService.BeginWriteDbLog().ConfigureAwait(false);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabCMS.EquipmentUsageRecord.Server v1"));
            }
            //app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>()
            //    .ApplicationStopped.Register(Uninstall);

            app.UseRouting();
            app.UseCookieJwtAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (!env.IsDevelopment())
            {
                app.UseConsulAsServiceProvider(nameof(EquipmentUsageRecord));
            }
        }


        
    }
}
