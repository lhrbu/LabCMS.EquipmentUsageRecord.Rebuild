using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Server.Services;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(BulkheadRetryAsyncFilter))]
    public class UsageRecordsController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        private readonly UsageRecordSoftDeleteLogService _softDeleteLogService;
        public UsageRecordsController(
            UsageRecordsRepository repository,
            UsageRecordSoftDeleteLogService softDeleteLogService)
        { 
            _repository = repository;
            _softDeleteLogService = softDeleteLogService;
        }
        [HttpGet]
        public IAsyncEnumerable<UsageRecord> GetAsync() =>
            _repository.UsageRecords.AsNoTracking().AsAsyncEnumerable();
        [HttpPost]
        public async ValueTask PostAsync(UsageRecord usageRecord)
        {
            await _repository.UsageRecords.AddAsync(usageRecord);
            await _repository.SaveChangesAsync();
        }

        [HttpPut]
        public async ValueTask PutAsync(UsageRecord usageRecord)
        {
            _repository.UsageRecords.Update(usageRecord);
            await _repository.SaveChangesAsync();
        }
        [HttpDelete]
        public async ValueTask DeleteById(int id)
        {
            UsageRecord? usageRecord = await _repository.UsageRecords.FindAsync(id);
            if (usageRecord is not null)
            {
                _repository.UsageRecords.Remove(usageRecord);
                await _repository.SaveChangesAsync();
                _softDeleteLogService.Logger.Information("{UsageRecord}", usageRecord);
            }
        }
    }
}
