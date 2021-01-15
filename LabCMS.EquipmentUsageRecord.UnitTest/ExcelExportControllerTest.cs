using LabCMS.EquipmentUsageRecord.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LabCMS.EquipmentUsageRecord.UnitTest
{
    public class ExcelExportControllerTest
    {
        [Fact]
        public void TestGet()
        {
            var controller = TestServerProvider.CreateController<ExcelExportController>();
            var fileResult = controller.Get() as FileStreamResult;
            Assert.NotNull(fileResult);
            using ExcelEngine excelEngine = new();
            IApplication app = excelEngine.Excel;
            app.DefaultVersion = ExcelVersion.Xlsx;
            Assert.NotNull(app.Workbooks.Open(fileResult!.FileStream));
        }
    }
}
