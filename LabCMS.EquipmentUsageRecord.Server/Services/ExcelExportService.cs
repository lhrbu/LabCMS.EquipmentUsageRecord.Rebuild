using LabCMS.EquipmentUsageRecord.Server.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using LabCMS.EquipmentUsageRecord.Server.Models;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class ExcelExportService
    {
        public Stream Export(IEnumerable<UsageRecord> usageRecords)
        {
            using ExcelEngine excelEngine = new();
            IApplication app = excelEngine.Excel;
            app.DefaultVersion = ExcelVersion.Xlsx;
            IWorkbook workbook = app.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets.First();
            IEnumerable<UsageRecordInExcel> usageRecordsInExcel = usageRecords
                .Select(item=>new UsageRecordInExcel(item));
            worksheet.ImportData(usageRecordsInExcel,1,1,true);
            FormatWorksheet(worksheet);
            Stream stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private void FormatWorksheet(IWorksheet worksheet)
        {
            IStyle headerCellStyle = worksheet.Rows.First().CellStyle;
            headerCellStyle.ColorIndex = ExcelKnownColors.Dark_blue;
            headerCellStyle.Font.Color = ExcelKnownColors.White;
            headerCellStyle.Font.Bold = true;
            SetAsNormalBorder(headerCellStyle);
            headerCellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            headerCellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

            worksheet.Columns[7].CellStyle.NumberFormat = "yyyy/mm/dd hh:mm";
            worksheet.Columns[8].CellStyle.NumberFormat = "yyyy/mm/dd hh:mm";
            foreach (IRange column in worksheet.Columns)
            {
                column.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                SetAsNormalBorder(column.CellStyle);
                column.AutofitColumns();
            }
        }

        private void SetAsNormalBorder(IStyle cellStyle)
        {
            cellStyle.Borders[ExcelBordersIndex.DiagonalDown].LineStyle = ExcelLineStyle.None;
            cellStyle.Borders[ExcelBordersIndex.DiagonalUp].LineStyle = ExcelLineStyle.None;
            cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Medium;
            cellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
            cellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Medium;
            cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Medium;
        }
    }
}