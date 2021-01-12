using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace LabCMS.EquipmentUsageRecord.Server.Models
{
    public class UsageRecordInExcel
    {
        private readonly UsageRecord _usageRecord;
        public UsageRecordInExcel(UsageRecord usageRecord)
        { _usageRecord = usageRecord;}

        public string? User => _usageRecord.User;
         [DisplayName("Test No")]
        public string? TestNo => _usageRecord.TestNo;

        [DisplayName("Equipment No")]
        public string? EquipmentNo => _usageRecord.EquipmentNo;

        [DisplayName("Equipment Name")]
        public string? EquipmentName=> _usageRecord.EquipmentHourlyRate?.EquipmentName;

        [DisplayName("Test Type")]
        public string? TestType => _usageRecord.TestType;
        [DisplayName("Project No")]
        public string? ProjectNo => _usageRecord.ProjectNo;
        [DisplayName("Project Name")]
        public string? ProjectName => _usageRecord.Project?.Name;

        [DisplayName("Start Time")]
        public DateTime StartTime => _usageRecord.StartTime.LocalDateTime;

        [DisplayName("End Time")]
        public DateTime EndTime=>_usageRecord.EndTime.LocalDateTime;
        public double Duration => Math.Round(_usageRecord.Duration,2);

        [DisplayName("Machine Category")]
        public string? MachineCategory => _usageRecord.EquipmentHourlyRate?.MachineCategory;
        [DisplayName("Hourly Rate")]
        public string? HourlyRate => _usageRecord.EquipmentHourlyRate?.HourlyRate;
    }

}