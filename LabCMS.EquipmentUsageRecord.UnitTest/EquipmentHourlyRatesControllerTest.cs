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
        public void TestGet()
        {
            var controller = TestServerProvider.CreateController<EquipmentHourlyRatesController>();
            foreach(var equipmentHourlyRate in controller.Get())
            { Assert.NotNull(equipmentHourlyRate); }
        }
        
    }
}
