using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;
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
        private readonly MachineDownRecordsRepository _repository;
        public MachineDownRecordsController(
            MachineDownRecordsRepository repository)
        { _repository = repository; }

        [HttpGet]
        public IAsyncEnumerable<MachineDownRecord> GetAllAsync() =>
            _repository.MachineDownRecords.OrderBy(item=>item.Id).AsAsyncEnumerable();

        [HttpPost]
        public async ValueTask RegisterRecord(MachineDownRecord record)
        {
            await _repository.MachineDownRecords.AddAsync(record);
            await _repository.SaveChangesAsync();
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
