using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.Models
{
    public class EquipmentHourlyRate
    {
        [Key]
        public string EquipmentNo { get; set; } = null!;
        public string EquipmentName { get; set; } = null!;
        public string MachineCategory { get; set; } = null!;
        public string HourlyRate { get; set; } = null!;
    }
}
