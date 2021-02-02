using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Services
{
    public class NotificationService
    {
        private readonly ConcurrentQueue<MachineDownRecord> _unclosedRecordsQueue = new();

        private readonly IServiceProvider _serviceProvider;
        private readonly EmailSendService _emailSendService;
        public NotificationService(
            IServiceProvider serviceProvider,
            EmailSendService emailSendService)
        { 
            _serviceProvider = serviceProvider;
            _emailSendService = emailSendService;
        }
        private readonly IEnumerable<string> _from = new[] { "machinedownrecord.center@hella.com" };


        public async Task ScheduleTasksForTodayAsync()
        {
            MachineDownRecordsRepository repository = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MachineDownRecordsRepository>();
            Notification[] notifications = repository.MachineDownRecords.Where(item => !item.MachineRepairedDate.HasValue)
                .Include(item=>item.User)
                .AsEnumerable()
                .Select(item=>new Notification(item,RefreshNotifyDate(item.MachineDownDate)))
                .OrderBy(item=>item.NotifyDateTimeOffset)
                .ToArray();

            foreach(Notification notification in notifications)
            {
                DateTimeOffset now = DateTimeOffset.Now;
                if(now<notification.NotifyDateTimeOffset.AddMinutes(2))
                { 
                    await Task.Delay(notification.NotifyDateTimeOffset - now);
                    await SendNotificationAsync(notification);
                }
            }
           
                
        }


        public async ValueTask SendNotificationAsync(Notification notification)
        {
            MachineDownRecord record = notification.MachineDownRecord;
            // MachineDownRecordsRepository repository = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MachineDownRecordsRepository>();
            // await repository.Entry(record).Reference(item => item.User).LoadAsync();
            IEnumerable<string> to = new[] { record!.User!.Email };
            string payload = $"[{DateTimeOffset.Now} INF]: {record.EquipmentNo} is down since {record.MachineDownDate}, need to change the state?";
            await _emailSendService.SendEmailAsync(_from, to,
                $"no-reply: machine down record notify at {DateTimeOffset.Now}",
                payload);
        }

        private DateTimeOffset RefreshNotifyDate(DateTimeOffset machineDownDate)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            return new DateTimeOffset(now.Year, now.Month, now.Day, machineDownDate.Hour, machineDownDate.Minute, machineDownDate.Second, now.Offset);
        }
    }
}