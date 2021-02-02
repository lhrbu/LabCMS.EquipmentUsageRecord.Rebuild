using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTimeOffset MachineDownDate {get;set;} = DateTimeOffset.Now;
        public DateTimeOffset? MachineRepairedDate {get;set;}
        public string? Comment {get;set;}
    }
}