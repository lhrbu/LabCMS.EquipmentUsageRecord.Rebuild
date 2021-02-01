using System;
using System.ComponentModel.DataAnnotations;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Models
{
    public class User
    {
        [Key]
        public string UserId {get;init;} = null!;
        public string Email {get;init;} =null!;
    }
}