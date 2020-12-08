using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.Models
{
    public class Project
    {
        [Key]
        public string No { get; set; } = null!;
        public string? Name { get; set; }
        public string FullName { get; set; } = null!;
    }
}
