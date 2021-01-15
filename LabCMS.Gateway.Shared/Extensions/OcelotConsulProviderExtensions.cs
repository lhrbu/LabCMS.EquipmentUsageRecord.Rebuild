using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.Gateway.Shared.Extensions
{
    public static class OcelotConsulProviderExtensions
    {
        public static IApplicationBuilder UseConsulAsServiceProvider(this IApplicationBuilder app, string serviceName)
        {
            IHostApplicationLifetime appLifeTime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            appLifeTime.ApplicationStarted.Register(() => app.RegisterToConsul(serviceName));
            appLifeTime.ApplicationStopped.Register(() => app.DeregisterFromConsul(serviceName));
            return app;
        }
        public static IApplicationBuilder RegisterToConsul(
            this IApplicationBuilder app,
            string serviceName)
        {
            using ConsulClient consulClient = new ConsulClient(options =>
                options.Address = new Uri("http://localhost:8500"));
            IEnumerable<Task<WriteResult>> RegisterTasks = app.ApplicationServices.GetUris()
                .Select(uri => consulClient.Agent.ServiceRegister(CreateServiceRegistration(serviceName, uri)));
            _ = Task.WhenAll(RegisterTasks).Result;
            return app;
        }

        public static IApplicationBuilder DeregisterFromConsul(
            this IApplicationBuilder app,
            string serviceName)
        {
            using ConsulClient consulClient = new (options =>
                options.Address = new Uri("http://localhost:8500"));
            IEnumerable<Task<WriteResult>> DeregisterTasks = app.ApplicationServices.GetUris().Select(
                uri => consulClient.Agent.ServiceDeregister(CreateServiceId(serviceName, uri)));
            _ = Task.WhenAll(DeregisterTasks).Result;
            return app;
        }
        private static AgentServiceRegistration CreateServiceRegistration(string name, Uri uri) =>
                new AgentServiceRegistration
                {
                    ID = CreateServiceId(name, uri),
                    Name = name,
                    Address = uri.Host,
                    Port = uri.Port
                };
        private static string CreateServiceId(string name, Uri uri) => Convert.ToBase64String(
                Encoding.Default.GetBytes($"{name}:{uri}"));
        private static IEnumerable<Uri> GetUris(this IServiceProvider serviceProvider) =>
            serviceProvider.GetRequiredService<IServer>()
                    .Features.Get<IServerAddressesFeature>().Addresses
                    .Select(item => new Uri(item));
    }
}
