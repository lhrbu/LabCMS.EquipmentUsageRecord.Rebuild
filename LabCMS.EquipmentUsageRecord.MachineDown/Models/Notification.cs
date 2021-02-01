using System;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Models
{
    public record Notification(MachineDownRecord MachineDownRecord,string HostUrl,DateTimeOffset TimeStamp);
}