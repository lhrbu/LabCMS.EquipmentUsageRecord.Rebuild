using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Models
{
    public record NotifiedToken
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset NotifiedDate { get; init; }
        [ForeignKey(nameof(MachineDownRecord))]
        public int MachineDownRecordId { get; init; }
        public MachineDownRecord? MachineDownRecord { get; set; }
    }
}
