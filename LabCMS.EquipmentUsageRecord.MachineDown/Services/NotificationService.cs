using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Services
{
    public class MachineDownRecordIdComparer : IEqualityComparer<MachineDownRecord?>
    {
        public bool Equals(MachineDownRecord? x, MachineDownRecord? y) =>
            x?.Id == y?.Id;

        public int GetHashCode([DisallowNull] MachineDownRecord obj) => obj.GetHashCode();
    }

    public class NotificationService
    {
        private readonly MachineDownRecordIdComparer _idComparer = new();
        private readonly IServiceProvider _serviceProvider;
        public NotificationService(IServiceProvider serviceProvider)
        { 
            _serviceProvider = serviceProvider;
        }
        //private readonly IEnumerable<string> _from = new[] { "machinedownrecord.center@hella.com" };
        private readonly IEnumerable<string> _from = new[] { "lihaoran228@163.com" };

        public async Task ScanAndSendNotificationAsync()
        {
            MachineDownRecordsRepository repository = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MachineDownRecordsRepository>();
            
            DateTimeOffset now = DateTimeOffset.Now;
            var notifiedRecords = repository.NotifiedTokens
                .Include(item => item.MachineDownRecord)
                .AsEnumerable()
                .Where(item=>item.NotifiedDate.Year==now.Year &&
                    item.NotifiedDate.Month == now.Month &&
                    item.NotifiedDate.Day == now.Day)
                .Select(item=>item.MachineDownRecord);

            IEnumerable<MachineDownRecord> records = repository.MachineDownRecords
                .Where(item => !item.MachineRepairedDate.HasValue)
                .Include(item=>item.User)
                .AsEnumerable()
                .Except(notifiedRecords,_idComparer)!;

            using EmailSendService emailSendService = _serviceProvider.GetRequiredService<EmailSendService>();
            foreach (MachineDownRecord record in records)
            { 
                await SendNotificationAsync(emailSendService,record);
                //await repository.NotifiedTokens.AddAsync(new() { 
                //    NotifiedDate = now, 
                //    MachineDownRecord = record });
            }
            //await repository.SaveChangesAsync();
        }


        public async ValueTask SendNotificationAsync(EmailSendService emailSendService,MachineDownRecord record)
        {
            IEnumerable<string> to = new[] { record!.User!.Email };
            string payload = $"[{DateTimeOffset.Now} INF]: Equipment {record.EquipmentNo} is down since {record.MachineDownDate}, need to change the state?";
            await emailSendService.SendEmailAsync(_from, to,
                $"no-reply: machine down record notify at {DateTimeOffset.Now}",
                payload);
            
        }
    }
}