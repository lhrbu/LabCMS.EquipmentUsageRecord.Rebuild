using LabCMS.EquipmentUsageRecord.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.Events
{
    public class UsageRecordPersisientEventArgs:EventArgs
    {
        public UsageRecordPersisientEventKind EventKind { get; }
        public UsageRecord UsageRecord { get; }
        public UsageRecordPersisientEventArgs(UsageRecord usageRecord, UsageRecordPersisientEventKind eventKind)
        {
            EventKind = eventKind;
            UsageRecord = usageRecord;
        }
    }
}
