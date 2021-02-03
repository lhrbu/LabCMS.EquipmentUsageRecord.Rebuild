using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;
using LabCMS.EquipmentUsageRecord.MachineDown.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using LabCMS.Gateway.Shared.Extensions;

namespace LabCMS.EquipmentUsageRecord.MachineDown
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LabCMS.EquipmentUsageRecord.MachineDownRecord", Version = "v1" });
            });
            services.AddSingleton<SmtpClientFactory>();
            services.AddTransient<SmtpClient>(serviceProvider =>
                serviceProvider.GetRequiredService<SmtpClientFactory>().Create());
            services.AddTransient<EmailSendService>();
            services.AddSingleton<NotificationService>();
            services.AddDbContextPool<MachineDownRecordsRepository>(options=>options
                .UseNpgsql(Configuration.GetConnectionString(nameof(MachineDownRecordsRepository))),64);
        }

        private readonly CancellationTokenSource _tokenSource = new();
        private async Task StartScanAsync(IApplicationBuilder app,TimeSpan? interval=null,int? startHour=null)
        {
            try
            {
                DateTimeOffset now = DateTimeOffset.Now;
                if (startHour.HasValue)
                {
                    DateTimeOffset targetDateTimeOffset = new (
                        now.Year, now.Month, now.Day, startHour.Value, 0, 0, now.Offset);
                    if (now < targetDateTimeOffset)
                    { await Task.Delay(targetDateTimeOffset - now); }
                }

                NotificationService notificationService = app.ApplicationServices
                    .GetRequiredService<NotificationService>();
                CancellationToken token = _tokenSource.Token;
                while (!token.IsCancellationRequested)
                {
                    await notificationService.ScanAndSendNotificationAsync();
                    await Task.Delay(interval??TimeSpan.FromDays(1), token);
                }
            }catch(Exception exception)
            {
                Console.WriteLine(exception);
                await Task.Delay(TimeSpan.FromMinutes(1));
                await StartScanAsync(app, interval ?? TimeSpan.FromDays(1), startHour);
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabCMS.EquipmentUsageRecord.MachineDownRecord v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            NotificationService notificationService = app.ApplicationServices
                .GetRequiredService<NotificationService>();

            lifetime.ApplicationStopping.Register(() => _tokenSource.Cancel());

            StartScanAsync(app).ConfigureAwait(false);
            
            if(!env.IsDevelopment()){
                app.UseConsulAsServiceProvider(nameof(MachineDownRecord));
            }
        }
    }
}
