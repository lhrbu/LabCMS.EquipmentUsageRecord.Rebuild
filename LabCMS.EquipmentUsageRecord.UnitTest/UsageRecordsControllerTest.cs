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
        public void TestGet()
        {
            var controller = TestServerProvider.CreateController<UsageRecordsController>();
            foreach(UsageRecord usageRecord in controller.Get())
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
                ProjectNo= "1394P.000001",
                EquipmentNo = "01-01",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(6)
            };
            var usageRecords = controller.Get().ToList();
            await controller.PostAsync(usageRecord);
            var usageRecordsAfterPost = controller.Get().ToList();
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
                ProjectNo = "1394P.000002",
                EquipmentNo = "01-01",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(6)
            };
            UsageRecord usageRecord2 = new()
            {
                User = Guid.NewGuid().ToString(),
                TestNo = "Test123",
                TestType = "TY",
                ProjectNo = "1394P.000003",
                EquipmentNo = "13-14",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(12)
            };
            var usageRecords = controller.Get();

            await controller.PostManyAsync(new[] { usageRecord, usageRecord2 });
            var usageRecordsAfterPost = controller.Get();
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
                ProjectNo = "1394P.000001",
                EquipmentNo = "01-01",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(6)
            };
            await controller.PostAsync(usageRecord);
            var usageRecords = controller.Get();

            usageRecord.Id = usageRecords.First(item => item.User == usageRecord.User).Id;
            usageRecord.User = Guid.NewGuid().ToString();
            await controller.PutAsync(usageRecord);
            var usageRecordsAfterPut = controller.Get();
            Assert.Contains(usageRecordsAfterPut, item => item.User == usageRecord.User);
            await controller.DeleteById(usageRecord.Id);

        }
    }
}
