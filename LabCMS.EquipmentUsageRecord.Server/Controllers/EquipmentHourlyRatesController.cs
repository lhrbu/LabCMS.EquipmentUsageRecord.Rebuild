using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.Server.Services;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentHourlyRatesController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        public EquipmentHourlyRatesController(
            UsageRecordsRepository repository)
        { _repository = repository; }
        [HttpGet]
        public IAsyncEnumerable<EquipmentHourlyRate> GetAsync() =>
            _repository.EquipmentHourlyRates.AsNoTracking().AsAsyncEnumerable();
        [HttpPost]
        public async ValueTask PostAsync(EquipmentHourlyRate equipmentHourlyRate)
        {
            await _repository.EquipmentHourlyRates.AddAsync(equipmentHourlyRate);
            await _repository.SaveChangesAsync();
        }
        [HttpPut]
        public async ValueTask PutAsync(EquipmentHourlyRate equipmentHourlyRate)
        {
            _repository.EquipmentHourlyRates.Update(equipmentHourlyRate);
            await _repository.SaveChangesAsync();
        }
        [HttpDelete]
        public async ValueTask DeleteByNoAsync(string equipmentNo)
        {
            EquipmentHourlyRate? equipmentHourlyRate = await 
                _repository.EquipmentHourlyRates.FindAsync(equipmentNo);
            if(equipmentHourlyRate is not null)
            {
                await _repository.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                if(!await _repository.UsageRecords.AnyAsync(item => item.EquipmentNo == equipmentNo))
                {
                    _repository.EquipmentHourlyRates.Remove(equipmentHourlyRate);
                }
                await _repository.Database.CommitTransactionAsync();
            }
        }
    }
}
