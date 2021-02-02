using System;
using System.ComponentModel.DataAnnotations;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Models
{
    public record User
    {
        [Key]
        public string UserId {get;init;} = null!;
        public string Email {get;init;} =null!;
    }
}