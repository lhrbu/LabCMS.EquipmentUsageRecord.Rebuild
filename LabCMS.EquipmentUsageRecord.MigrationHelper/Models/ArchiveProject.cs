using LabCMS.EquipmentUsageRecord.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MigrationHelper.Models
{
    public class ArchiveProject
    {
        public string? No {get;set;}
        public string? Name {get;set;}
        public string? FullName {get;set;}
    }
}