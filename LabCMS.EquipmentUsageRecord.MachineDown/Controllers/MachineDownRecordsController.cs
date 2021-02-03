using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;
using LabCMS.EquipmentUsageRecord.MachineDown.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineDownRecordsController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly MachineDownRecordsRepository _repository;
        public MachineDownRecordsController(
            NotificationService notificationService,
            MachineDownRecordsRepository repository)
        { 
            _notificationService = notificationService;
            _repository = repository; 
        }

        [HttpGet]
        public IAsyncEnumerable<MachineDownRecord> GetAllAsync() =>
            _repository.MachineDownRecords.OrderBy(item=>item.Id).AsAsyncEnumerable();

        [HttpPost]
        public async ValueTask RegisterRecord([FromBody]MachineDownRecord record,
            [FromServices]EmailSendService emailSendService)
        {
            var recordEntry = await _repository.MachineDownRecords.AddAsync(record);
            await _repository.SaveChangesAsync();
            await recordEntry.Reference(item=>item.User).LoadAsync();
            await _repository.NotifiedTokens.AddAsync(new() { 
                   NotifiedDate = DateTimeOffset.Now, 
                   MachineDownRecord = recordEntry.Entity });
            await _notificationService.SendNotificationAsync(emailSendService,recordEntry.Entity);
        }

        [HttpPut]
        public async ValueTask UpdateRecord(MachineDownRecord record)
        {
            _repository.MachineDownRecords.Update(record);
            await _repository.SaveChangesAsync();
        }

        [HttpDelete]
        public async ValueTask<IActionResult> DeleteByIdAsync(int id)
        {
            MachineDownRecord? record = await _repository.MachineDownRecords.FindAsync(id);
            if (record is not null)
            {
                _repository.MachineDownRecords.Remove(record);
                await _repository.SaveChangesAsync();
                return Ok();
            }
            else { return BadRequest(); }
        }

    }
}
