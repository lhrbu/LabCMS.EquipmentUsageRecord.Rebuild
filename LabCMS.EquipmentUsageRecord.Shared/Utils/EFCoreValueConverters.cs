using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LabCMS.EquipmentUsageRecord.Shared.Utils
{
    public static class EFCoreValueConverters
    {
        public static ValueConverter<DateTimeOffset, long> DataTimeOffsetUtcSecondsConverter { get; } = new(
                    dateTimeOffset => dateTimeOffset.ToUnixTimeSeconds(),
                    offsetSeconds => DateTimeOffset.FromUnixTimeSeconds(offsetSeconds) );
        
        public static ValueConverter<DateTimeOffset?,long?> NullableDateTimeOffsetUtcSecondsConverter{get;}=new(
            dateTimeOffset=>dateTimeOffset.HasValue?dateTimeOffset.Value.ToUnixTimeSeconds():null,
            offsetSeconds => offsetSeconds.HasValue?DateTimeOffset.FromUnixTimeSeconds(offsetSeconds.Value):null
        );
    }
}
