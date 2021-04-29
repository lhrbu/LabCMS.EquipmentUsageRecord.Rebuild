using LabCMS.EquipmentUsageRecord.Server.Services;
using LabCMS.EquipmentUsageRecord.Shared.Extensions;
using LabCMS.EquipmentUsageRecord.Shared.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Raccoon.Devkits.DynamicProxy;
using Raccoon.Devkits.Gateway.Client;
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
            services.AddUsageRecordsRepository(Configuration);

            services.AddTransient<ExcelExportService>();
            services.AddTransient<DynamicQueryService>();
            services.AddEasyNetQ();
            services.AddSingleton<PythonDynamicQueryService>(provider=>
                new(provider, @"C:\Users\lhrbuxiaoxin\AppData\Local\Programs\Python\Python39\python39.dll"));
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabCMS.EquipmentUsageRecord.Server v1"));
            }

            app.UseRouting();

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
