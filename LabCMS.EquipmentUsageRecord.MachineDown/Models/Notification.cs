using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Models
{
    public record Notification(
        MachineDownRecord MachineDownRecord,
        DateTimeOffset NotifyDate);
}
