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
        public async ValueTask TestGetAsync()
        {
            var controller = TestServerProvider.CreateController<EquipmentHourlyRatesController>();
            await foreach(var equipmentHourlyRate in controller.GetAsync())
            { Assert.NotNull(equipmentHourlyRate); }
        }

        [Fact]
        public async ValueTask TestPostAsync()
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
        public async ValueTask TestPutAsync()
        {
            var controller = TestServerProvider.CreateController<EquipmentHourlyRatesController>();
            EquipmentHourlyRate equipmentHourlyRate = new()
            {
                EquipmentName = "EQ1PutNAME",
                EquipmentNo = "EQ1Put",
                HourlyRate = "A",
                MachineCategory = "DPE1"
            };
            await controller.PostAsync(equipmentHourlyRate);
            var items = controller.GetAsync().ToEnumerable();

            EquipmentHourlyRate changedEquipment = new()
            {
                EquipmentName = "EQ1PutNameAfter",
                EquipmentNo = equipmentHourlyRate.EquipmentNo,
                HourlyRate = equipmentHourlyRate.HourlyRate,
                MachineCategory = equipmentHourlyRate.MachineCategory
            };
            await controller.PutAsync(changedEquipment);
            var itemsAfterPut = controller.GetAsync().ToEnumerable();
            Assert.Equal(changedEquipment.EquipmentName, itemsAfterPut.First(item => item.EquipmentNo == changedEquipment.EquipmentNo)
                .EquipmentName);
        }
    }
}
