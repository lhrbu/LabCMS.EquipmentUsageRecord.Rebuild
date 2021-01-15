using LabCMS.EquipmentUsageRecord.Server.Controllers;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LabCMS.EquipmentUsageRecord.UnitTest
{
    public class EquipmentHourlyRatesControllerTest
    {
        [Fact]
        public async Task TestGetAsync()
        {
            var controller = TestServerProvider.CreateController<EquipmentHourlyRatesController>();
            await foreach(var equipmentHourlyRate in controller.GetAsync())
            { Assert.NotNull(equipmentHourlyRate); }
        }

        [Fact]
        public async Task TestPostAsync()
        {
            var controller = TestServerProvider.CreateController<EquipmentHourlyRatesController>();
            EquipmentHourlyRate equipmentHourlyRate = new()
            {
                EquipmentName = "EQ1PostNAME",
                EquipmentNo = "EQ1Post",
                HourlyRate = "A",
                MachineCategory = "DPE1"
            };
            var items =  controller.GetAsync().ToEnumerable();

            await controller.PostAsync(equipmentHourlyRate);
            var itemsAfterPost = controller.GetAsync().ToEnumerable();
            Assert.Contains(itemsAfterPost, item => item.EquipmentNo == equipmentHourlyRate.EquipmentNo);

            await controller.DeleteByNoAsync(equipmentHourlyRate.EquipmentNo);
            var itemsAfterDelete = controller.GetAsync().ToEnumerable();
            Assert.DoesNotContain(itemsAfterDelete, item => item.EquipmentNo == equipmentHourlyRate.EquipmentNo);
        }

        [Fact]
        public async Task TestPutAsync()
        {
            var controller = TestServerProvider.CreateController<EquipmentHourlyRatesController>();
            EquipmentHourlyRate equipmentHourlyRate = new()
            {
                EquipmentName = Guid.NewGuid().ToString(),
                EquipmentNo = "EQ1Put",
                HourlyRate = "A",
                MachineCategory = "DPE1"
            };
            await controller.PostAsync(equipmentHourlyRate);
            equipmentHourlyRate.EquipmentName = Guid.NewGuid().ToString();
            await controller.PutAsync(equipmentHourlyRate);
            var itemsAfterPut = controller.GetAsync().ToEnumerable();
            Assert.Equal(equipmentHourlyRate.EquipmentName, itemsAfterPut.First(item => item.EquipmentNo == equipmentHourlyRate.EquipmentNo)
                .EquipmentName);
            await controller.DeleteByNoAsync(equipmentHourlyRate.EquipmentNo);
        }
    }
}
