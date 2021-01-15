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
        public async Task TestGetAsync()
        {
            var controller = TestServerProvider
                .CreateController<ProjectsController>();
            await foreach(Project project in controller.GetAsync())
            { Assert.NotNull(project);}
        }

        [Fact]
        public async Task TestPostAndDeleteAsync()
        {
            var controller = TestServerProvider.CreateController<ProjectsController>();
            Project project = new() { No = "1394E.XXXYYYPost", Name = "TestProject", NameInFIN = "TestProjectInFIN" };
            var projects = controller.GetAsync().ToEnumerable();
            
            await controller.PostAsync(project);
            var projectsAfterPost = controller.GetAsync().ToEnumerable();
            Assert.Contains(projectsAfterPost, item => item.No == project.No);

            await controller.DeleteByName(project.Name);
            var projectsAfterDelete = controller.GetAsync().ToEnumerable();
            Assert.DoesNotContain(projectsAfterDelete, item => item.No == project.No);
        }

        [Fact]
        public async Task TestPutAsync()
        {
            var controller = TestServerProvider.CreateController<ProjectsController>();
            Project project = new() { No = "1394E.XXXYYYUpdate", Name = Guid.NewGuid().ToString(), NameInFIN = "TestProjectInFIN" };
            await controller.PostAsync(project);
            project.Name = Guid.NewGuid().ToString();

            await controller.PutAsync(project);
            var projectsAfterPut = controller.GetAsync().ToEnumerable();
            Assert.Equal(project.Name, projectsAfterPut.First(item => item.No == project.No).Name);
            await controller.DeleteByName(project.Name);
        }
    }
}
