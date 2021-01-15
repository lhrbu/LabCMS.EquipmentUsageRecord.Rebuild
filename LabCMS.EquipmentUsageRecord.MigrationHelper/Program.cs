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
            using Stream projectsStream = File.OpenRead("projects.json");
            IEnumerable<Project> projects = JsonSerializer.DeserializeAsync<IEnumerable<Project>>(projectsStream).Result!;
            using Stream archiveProjectsStream = File.OpenRead("archiveProjects.json");
            var archiveProjects = JsonSerializer.DeserializeAsync<IEnumerable<ArchiveProject>>(archiveProjectsStream).Result;
            using Stream archiveStream = File.OpenRead("archive.json");
            var archiveRecords =JsonSerializer.DeserializeAsync<IEnumerable<ArchiveUsageRecord>>(archiveStream).Result!;
            try{
            var usageRecords = archiveRecords.Select(item => new UsageRecord
            {
                User = item.User!,
                TestNo = item.TestNo!,
                EquipmentNo = item.EquipmentNo!,
                TestType = item.TestType,
                ProjectNo = FindProjectNoByFullName(item.ProjectName!,archiveProjects!),
                StartTime = item.StartTime!.Value,
                EndTime = item.EndTime!.Value
            });
            using Stream outputStream = File.OpenWrite("output.json");
            JsonSerializer.SerializeAsync<IEnumerable<UsageRecord>>(outputStream, usageRecords,
                new() { WriteIndented = true });
            var r = usageRecords.Where(record=>!projects.Any(project=>project.No==record.ProjectNo)).ToList();
            var p = r.Select(r=>r.ProjectNo).Distinct();

            SendToDbAsync(usageRecords).Wait();

            }catch(Exception e){Console.WriteLine(e);}
        }

        static string FindProjectNoByFullName(string archiveProjectFullName,
            IEnumerable<ArchiveProject> archiveProjects)
        {
            ArchiveProject archiveProject = archiveProjects.First(item=>item.FullName==archiveProjectFullName);
            return archiveProject.No!;
        }

        static async Task SendToDbAsync(IEnumerable<UsageRecord> usageRecords)
        {
            using HttpClient httpClient = new();
            var list = usageRecords.OrderBy(item=>item.StartTime);
            var res = await httpClient.PostAsJsonAsync("http://10.99.159.149:18000/api/UsageRecords/Many", list);
            Console.WriteLine(res);
        }
    }
}
