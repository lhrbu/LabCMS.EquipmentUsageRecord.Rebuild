using LabCMS.EquipmentUsageRecord.Server.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LabCMS.EquipmentUsageRecord.Server", Version = "v1" });
            });
            services.AddDbContextPool<UsageRecordsRepository>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString(nameof(UsageRecordsRepository)));
                options.LogTo(PostDbLog, LogLevel.Information).EnableSensitiveDataLogging();
            },256);
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            WriteDbLog().ConfigureAwait(false);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabCMS.EquipmentUsageRecord.Server v1"));
            }
            app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>()
                .ApplicationStopped.Register(Uninstall);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void Uninstall()
        {
            while (_logChannel.Reader.Count > 0)
            {
                if (_logChannel.Reader.TryRead(out string? item))
                {
                    if (item is not null)
                    { _logWriter.WriteLine(item); }
                }
            }
            _logWriter.Dispose();
        }

        private readonly StreamWriter _logWriter = new("db.log", append: true) { AutoFlush = true };
        private Channel<string> _logChannel = Channel.CreateUnbounded<string>();
        private async Task WriteDbLog()
        {
            while (true)
            {
                string log = await _logChannel.Reader.ReadAsync();
                _logWriter.WriteLine(log);
            }
        }
        private void PostDbLog(string log) => _logChannel.Writer.TryWrite(log);
    }
}
