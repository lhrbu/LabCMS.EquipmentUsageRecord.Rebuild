using LabCMS.EquipmentUsageRecord.Server.Repositories;
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
    public class UsageRecordsController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        public UsageRecordsController(UsageRecordsRepository repository)
        { _repository = repository; }
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
            if(usageRecord is not null)
            {
                _repository.UsageRecords.Remove(usageRecord);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
