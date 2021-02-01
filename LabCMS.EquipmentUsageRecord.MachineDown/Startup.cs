using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.MachineDown.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace LabCMS.EquipmentUsageRecord.MachineDown
{
    public class Startup
    {
        private SmtpClient CreateSmtpClient()
        {
            SmtpClient smtpClient = new();
            smtpClient.Connect("", 25, SecureSocketOptions.None);
            smtpClient.Authenticate("liha52", "2112358LHR/");
            return smtpClient;
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LabCMS.EquipmentUsageRecord.MachineDownRecord", Version = "v1" });
            });
            services.AddSingleton<SmtpClient>(CreateSmtpClient());
            services.AddSingleton<EmailSendService>();
            services.AddSingleton<NotificationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            Timer timer = new(obj => _=(obj as NotificationService)!.ScheduleTasksForTodayAsync(), 
                notificationService, 
                TimeSpan.Zero, 
                TimeSpan.FromDays(1));
        }
    }
}
