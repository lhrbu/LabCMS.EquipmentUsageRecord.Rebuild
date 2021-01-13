using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Server.Services;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchInteropController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        private readonly IElasticSearchInteropService _elasticSearch;
        public ElasticSearchInteropController(
            UsageRecordsRepository repository,
            IElasticSearchInteropService elasticSearch)
        {
            _repository = repository;
            _elasticSearch = elasticSearch;
        }

        [HttpPost]
        public async ValueTask SyncElasticeSearchWithDatabaseAsync()
        {
            IEnumerable<UsageRecord> usageRecords = _repository.UsageRecords.AsNoTracking();
            IEnumerable<UsageRecord> usageRecordsInES = await _elasticSearch.SearchAllAsync();
            UsageRecordIdEqualityComparer comparer = new();
            IEnumerable<UsageRecord> recordsInESNeedToDelete = usageRecordsInES.Except(usageRecords, comparer);
            IEnumerable<UsageRecord> recordsNeedToAddToES = usageRecords.Except(usageRecordsInES, comparer);
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
