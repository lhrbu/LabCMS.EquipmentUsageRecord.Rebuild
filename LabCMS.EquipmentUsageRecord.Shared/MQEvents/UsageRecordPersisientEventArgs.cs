using LabCMS.EquipmentUsageRecord.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.MQEvents
{
    public class UsageRecordPersisientEventArgs : EventArgs
    {
        public UsageRecordsChangeEventKind EventKind { get; }
        public UsageRecord UsageRecord { get; }
        public UsageRecordPersisientEventArgs(UsageRecord usageRecord, UsageRecordsChangeEventKind eventKind)
        {
            EventKind = eventKind;
            UsageRecord = usageRecord;
        }
    }
}
