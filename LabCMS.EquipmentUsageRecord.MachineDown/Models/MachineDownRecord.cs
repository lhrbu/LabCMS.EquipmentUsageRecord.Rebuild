using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabCMS.EquipmentUsageRecord.Shared.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Models
{
    public record MachineDownRecord
    {
        [Key]
        public int Id {get;set;}

        [ForeignKey(nameof(User))]
        public string UserId {get;set;}=null!;
        public User? User {get;set;}
        public string EquipmentNo {get;set;}=null!;
        [JsonConverter(typeof(JsonConverters.DateTimeOffsetJsonConverter))]
        public DateTimeOffset MachineDownDate {get;set;} = DateTimeOffset.Now;
        [JsonConverter(typeof(JsonConverters.NullableDateTimeOffsetJsonConverter))]
        public DateTimeOffset? MachineRepairedDate {get;set;}
        public string Comment {get;set;}=null!;
    }
}