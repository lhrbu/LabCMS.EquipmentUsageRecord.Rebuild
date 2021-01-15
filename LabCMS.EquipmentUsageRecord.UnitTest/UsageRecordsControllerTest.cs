using LabCMS.EquipmentUsageRecord.Server.Controllers;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LabCMS.EquipmentUsageRecord.UnitTest
{
    public class UsageRecordsControllerTest
    {
        [Fact]
        public async Task TestGetAsync()
        {
            var controller = TestServerProvider.CreateController<UsageRecordsController>();
            await foreach(UsageRecord usageRecord in controller.GetAsync())
            { Assert.NotNull(usageRecord); }
        }

        [Fact]
        public async Task TestPostAsync()
        {
            var controller = TestServerProvider.CreateController<UsageRecordsController>();
            UsageRecord usageRecord = new()
            {
                User = Guid.NewGuid().ToString(),
                TestNo = "Test123",
                TestType = "TY",
                ProjectNo= "1394E.X00001",
                EquipmentNo = "01-01",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(6)
            };
            var usageRecords = controller.GetAsync().ToEnumerable().ToList();
            await controller.PostAsync(usageRecord);
            var usageRecordsAfterPost = controller.GetAsync().ToEnumerable().ToList();
            Assert.Contains(usageRecordsAfterPost, item => item.User == usageRecord.User);

            await controller.DeleteById(usageRecordsAfterPost.First(item => item.User == usageRecord.User).Id);
        }


        [Fact]
        public async Task TestPostManyAsync()
        {
            var controller = TestServerProvider.CreateController<UsageRecordsController>();
            UsageRecord usageRecord = new()
            {
                User = Guid.NewGuid().ToString(),
                TestNo = "Test123",
                TestType = "TY",
                ProjectNo = "1394E.X00001",
                EquipmentNo = "01-01",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(6)
            };
            UsageRecord usageRecord2 = new()
            {
                User = Guid.NewGuid().ToString(),
                TestNo = "Test123",
                TestType = "TY",
                ProjectNo = "1394E.X00002",
                EquipmentNo = "01-01",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(12)
            };
            var usageRecords = controller.GetAsync().ToEnumerable();

            await controller.PostManyAsync(new[] { usageRecord, usageRecord2 });
            var usageRecordsAfterPost = controller.GetAsync().ToEnumerable();
            Assert.Contains(usageRecordsAfterPost, item => item.User == usageRecord.User);
            Assert.Contains(usageRecordsAfterPost, item => item.User == usageRecord2.User);
            await controller.DeleteById(usageRecordsAfterPost.First(item => item.User == usageRecord.User).Id);
            await controller.DeleteById(usageRecordsAfterPost.First(item => item.User == usageRecord2.User).Id);
        }

        [Fact]
        public async Task TestPutAsync()
        {
            var controller = TestServerProvider.CreateController<UsageRecordsController>();
            UsageRecord usageRecord = new()
            {
                User = Guid.NewGuid().ToString(),
                TestNo = "Test123",
                TestType = "TY",
                ProjectNo = "1394E.X00001",
                EquipmentNo = "01-01",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(6)
            };
            await controller.PostAsync(usageRecord);
            var usageRecords = controller.GetAsync().ToEnumerable();

            usageRecord.Id = usageRecords.First(item => item.User == usageRecord.User).Id;
            usageRecord.User = Guid.NewGuid().ToString();
            await controller.PutAsync(usageRecord);
            var usageRecordsAfterPut = controller.GetAsync().ToEnumerable();
            Assert.Contains(usageRecordsAfterPut, item => item.User == usageRecord.User);
            await controller.DeleteById(usageRecord.Id);

        }
    }
}
