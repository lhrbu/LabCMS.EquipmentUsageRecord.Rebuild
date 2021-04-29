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
using LabCMS.EquipmentUsageRecord.Shared.Repositories;
using EasyNetQ;
using LabCMS.EquipmentUsageRecord.Shared.Events;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsageRecordsController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        private readonly UsageRecordSoftDeleteLogService _softDeleteLogService;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;
        public UsageRecordsController(
            UsageRecordsRepository repository,
            UsageRecordSoftDeleteLogService softDeleteLogService,
            IBus bus,
            IConfiguration configuration)
        { 
            _repository = repository;
            _softDeleteLogService = softDeleteLogService;
            _configuration = configuration;
            _bus = bus;
        }

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
            await _repository.UsageRecords.AddAsync(usageRecord);
            await _repository.SaveChangesAsync();
            await PublishPersisientEventAsync(usageRecord, UsageRecordsChangeEventKind.Add);

        }

        [HttpPost("Many")]
        public async ValueTask PostManyAsync(IEnumerable<UsageRecord> usageRecords)
        {
            await _repository.UsageRecords.AddRangeAsync(usageRecords);
            await _repository.SaveChangesAsync();
            foreach(UsageRecord usageRecord in usageRecords)
            {
                await PublishPersisientEventAsync(usageRecord, UsageRecordsChangeEventKind.Add);
            }
        }

        [HttpPut]
        public async ValueTask PutAsync(UsageRecord usageRecord)
        {
            _repository.UsageRecords.Update(usageRecord);
            await _repository.SaveChangesAsync();
            await PublishPersisientEventAsync(usageRecord, UsageRecordsChangeEventKind.Update);

        }
        [HttpDelete("{id}")]
        public async ValueTask DeleteById(int id)
        {
            UsageRecord? usageRecord = await _repository.UsageRecords.FindAsync(id);
            if (usageRecord is not null)
            {
                _repository.UsageRecords.Remove(usageRecord);
                await _repository.SaveChangesAsync();
                await PublishPersisientEventAsync(usageRecord, UsageRecordsChangeEventKind.Delete);
            }
        }

        private async ValueTask PublishPersisientEventAsync(UsageRecord usageRecord, UsageRecordsChangeEventKind eventKind)
            => await _bus.PubSub.PublishAsync<UsageRecordPersisientEventArgs>(
                new(usageRecord, eventKind))
                    .ContinueWith(task =>{if (task.IsFaulted && task.Exception != null) { throw task.Exception; }});
    }

    
}
