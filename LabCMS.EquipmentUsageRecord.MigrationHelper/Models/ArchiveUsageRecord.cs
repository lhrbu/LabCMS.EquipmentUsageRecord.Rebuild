using LabCMS.EquipmentUsageRecord.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MigrationHelper.Models
{
    public class ArchiveUsageRecord
    {
        public Guid? Id { get; set; }
        public string? User { get; set; }
        public string? TestNo { get; set; }
        public string? EquipmentNo { get; set; }
        public string? TestType { get; set; }
        public string? ProjectName { get; set; }

        [JsonConverter(typeof(JsonConverters.DateTimeOffsetJsonConverter))]
        public DateTimeOffset? StartTime { get; set; }

        [JsonConverter(typeof(JsonConverters.DateTimeOffsetJsonConverter))]
        public DateTimeOffset? EndTime { get; set; }

        [JsonIgnore]
        public double? Duration => (EndTime - StartTime)?.TotalHours;
    }
}
