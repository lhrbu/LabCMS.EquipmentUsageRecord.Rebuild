using LabCMS.EquipmentUsageRecord.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public interface IElasticSearchInteropService
    {
        ValueTask IndexAsync(UsageRecord usageRecord);
        ValueTask IndexManyAsync(IEnumerable<UsageRecord> usageRecords);
        ValueTask<IEnumerable<UsageRecord>> SearchAllAsync();
        ValueTask RemoveByIdAsync(int id);
        ValueTask RemoveManyAsync(IEnumerable<UsageRecord> usageRecords);
    }
}
