using LabCMS.EquipmentUsageRecord.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.Devkits.UnitTestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.UnitTest
{
    public static class TestServerProvider
    {
        public static TestServer Instance { get; } = new(WebHost.CreateDefaultBuilder().UseStartup<Startup>().UseEnvironment("Development"));

        public static ControllerCreateService ControllerCreateService { get; } = new();


        public static TController CreateController<TController>() where TController : ControllerBase
        {
            IServiceProvider scopedServiceProvider = Instance.Services.CreateScope().ServiceProvider;
            return ControllerCreateService.Create<TController>(scopedServiceProvider);
        }
        
    }
}
