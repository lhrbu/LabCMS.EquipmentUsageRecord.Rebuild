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
using Microsoft.Extensions.Configuration;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(BulkheadRetryAsyncFilter))]
    public class UsageRecordsController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        private readonly UsageRecordSoftDeleteLogService _softDeleteLogService;
        private readonly IElasticSearchInteropService _elasticSearch;
        private readonly IConfiguration _configuration;
        public UsageRecordsController(
            UsageRecordsRepository repository,
            UsageRecordSoftDeleteLogService softDeleteLogService,
            IElasticSearchInteropService elasticSearch,
            IConfiguration configuration)
        { 
            _repository = repository;
            _softDeleteLogService = softDeleteLogService;
            _elasticSearch = elasticSearch;
            _configuration = configuration;
        }
        public bool EnableElasticSearch => _configuration.GetValue<bool>(nameof(EnableElasticSearch));

        private async ValueTask LoadReferences(UsageRecord usageRecord)
        {
            await _repository.Entry(usageRecord).Reference(item => item.Project).LoadAsync();
            await _repository.Entry(usageRecord).Reference(item => item.EquipmentHourlyRate).LoadAsync();
        }

        [HttpGet]
        public IAsyncEnumerable<UsageRecord> GetAsync() =>
            _repository.UsageRecords.OrderBy(item=>item.Id).AsNoTracking().AsAsyncEnumerable();
        [HttpPost]
        public async ValueTask PostAsync(UsageRecord usageRecord)
        {
            try
            {
                await _repository.UsageRecords.AddAsync(usageRecord);
                await _repository.SaveChangesAsync();
                await LoadReferences(usageRecord);
                _ = _elasticSearch.IndexAsync(usageRecord).ConfigureAwait(false);
            }catch(Exception e) { throw; }
            
        }

        [HttpPost("Many")]
        public async ValueTask PostManyAsync(IEnumerable<UsageRecord> usageRecords)
        {
            await _repository.UsageRecords.AddRangeAsync(usageRecords);
            await _repository.SaveChangesAsync();

            foreach (UsageRecord usageRecord in usageRecords)
            { await LoadReferences(usageRecord); }
            _ = _elasticSearch.IndexManyAsync(usageRecords).ConfigureAwait(false);

        }

        [HttpPut]
        public async ValueTask PutAsync(UsageRecord usageRecord)
        {
            _repository.UsageRecords.Update(usageRecord);
            await _repository.SaveChangesAsync();
            await LoadReferences(usageRecord);
            _ = _elasticSearch.IndexAsync(usageRecord).ConfigureAwait(false);

        }
        [HttpDelete("{id}")]
        public async ValueTask DeleteById(int id)
        {
            UsageRecord? usageRecord = await _repository.UsageRecords.FindAsync(id);
            if (usageRecord is not null)
            {
                _ = _elasticSearch.RemoveByIdAsync(id).ConfigureAwait(false);
                _repository.UsageRecords.Remove(usageRecord);
                await _repository.SaveChangesAsync();
                _softDeleteLogService.Logger.Information("{UsageRecord}", usageRecord);
            }
        }
    }

    
}
