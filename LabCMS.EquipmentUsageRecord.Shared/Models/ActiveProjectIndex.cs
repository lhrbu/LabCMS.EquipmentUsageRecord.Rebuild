using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.Models
{
    public class ActiveProjectIndex
    {
        [Key]
        [ForeignKey(nameof(Project))]
        public string No { get; set; } = null!;
        public Project? Project { get; set; }
    }
}
