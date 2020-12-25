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
        private readonly ElasticSearchInteropService _elasticSearch;
        public UsageRecordsController(
            UsageRecordsRepository repository,
            UsageRecordSoftDeleteLogService softDeleteLogService,
            ElasticSearchInteropService elasticSearch)
        { 
            _repository = repository;
            _softDeleteLogService = softDeleteLogService;
            _elasticSearch = elasticSearch;
        }
        [HttpGet]
        public IAsyncEnumerable<UsageRecord> GetAsync() =>
            _repository.UsageRecords.AsNoTracking().AsAsyncEnumerable();
        [HttpPost]
        public async ValueTask PostAsync(UsageRecord usageRecord)
        {
            _ = _elasticSearch.IndexAsync(usageRecord).ConfigureAwait(false);
            await _repository.UsageRecords.AddAsync(usageRecord);
            await _repository.SaveChangesAsync();
        }

        [HttpPut]
        public async ValueTask PutAsync(UsageRecord usageRecord)
        {
            _ = _elasticSearch.IndexAsync(usageRecord).ConfigureAwait(false);
            _repository.UsageRecords.Update(usageRecord);
            await _repository.SaveChangesAsync();
        }
        [HttpDelete]
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

        [HttpPost("SyncWithElasticeSearch")]
        public async ValueTask SyncWithElasticeSearchAsync()
        {
            IEnumerable<UsageRecord> usageRecords = _repository.UsageRecords.AsNoTracking();
            IEnumerable<UsageRecord> usageRecordsInES = await _elasticSearch.SearchAllAsync();

            UsageRecordIdEqualityComparer comparer = new();
            IEnumerable<UsageRecord> recordsInESNeedToDelete = usageRecordsInES.Except(usageRecords,comparer);
            IEnumerable<UsageRecord> recordsNeedToAddToES = usageRecords.Except(usageRecordsInES,comparer);

            if (recordsInESNeedToDelete.Any())
            {
                await _elasticSearch.RemoveManyAsync(recordsInESNeedToDelete);
            }
            if (recordsNeedToAddToES.Any())
            {
                await _elasticSearch.IndexManyAsync(recordsNeedToAddToES);
            }
        }
    }

    public class UsageRecordIdEqualityComparer : IEqualityComparer<UsageRecord>
    {
        public bool Equals(UsageRecord? x, UsageRecord? y)
        {
            if ((x is null) && (y is null)) { return false; }
            else { return x?.Id == y?.Id; }
        }
        public int GetHashCode(UsageRecord obj) => obj.Id.GetHashCode();
    }
}
