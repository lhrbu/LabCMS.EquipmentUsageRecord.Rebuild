using LabCMS.EquipmentUsageRecord.MigrationHelper.Models;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MigrationHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            using Stream projectsStream = File.OpenRead("projects");
            var projects = JsonSerializer.DeserializeAsync<IEnumerable<Project>>(projectsStream).Result!;
            using Stream archiveStream = File.OpenRead("archive.json");
            var archiveRecords =JsonSerializer.DeserializeAsync<IEnumerable<ArchiveUsageRecord>>(archiveStream).Result!;
            
            var usageRecords = archiveRecords.Select(item => new UsageRecord
            {
                User = item.User!,
                TestNo = item.TestNo!,
                EquipmentNo = item.EquipmentNo!,
                TestType = item.TestType,
                ProjectNo = projects.First(p=>p.Name==item.ProjectName).No,
                StartTime = item.StartTime!.Value,
                EndTime = item.EndTime!.Value
            });
            using Stream outputStream = File.OpenWrite("output.json");
            JsonSerializer.SerializeAsync<IEnumerable<UsageRecord>>(outputStream, usageRecords,
                new() { WriteIndented = true });
            SendToDbAsync(usageRecords).Wait();
        }

        static async Task SendToDbAsync(IEnumerable<UsageRecord> usageRecords)
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsJsonAsync("http://localhost:5000/api/UsageRecords/Many", usageRecords);
        }
    }
}
