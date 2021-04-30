using LabCMS.EquipmentUsageRecord.Server.Controllers;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace LabCMS.EquipmentUsageRecord.UnitTest
{
    public class ProjectsControllerTest
    {
       

       [Fact]
        public void TestGet()
        {
            var controller = TestServerProvider
                .CreateController<ProjectsController>();
            foreach(Project project in controller.Get())
            { Assert.NotNull(project);}
        }

        [Fact]
        public void TestGetActive()
        {
            var controller = TestServerProvider
                .CreateController<ProjectsController>();
            foreach (Project project in controller.Get())
            { Assert.NotNull(project); }
        }
    }
}
