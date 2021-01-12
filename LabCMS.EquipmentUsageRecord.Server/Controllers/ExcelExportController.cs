using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Server.Services;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelExportController:ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        private readonly ExcelExportService _excelExportService;
        public ExcelExportController(
            UsageRecordsRepository repository,
            ExcelExportService excelExportService)
        { 
            _repository = repository;
            _excelExportService = excelExportService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Stream stream = _excelExportService.Export(
                _repository.UsageRecords.Include(item=>item.Project).Include(item=>item.EquipmentHourlyRate));
            return File(stream,"text/plain", "EquipmentUsageRecord.xlsx");
        }
    }
}