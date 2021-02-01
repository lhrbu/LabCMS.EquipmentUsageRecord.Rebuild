using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Services
{
    public class NotificationService
    {
        private readonly ConcurrentQueue<MachineDownRecord> _unclosedRecordsQueue = new();

        private readonly MachineDownRecordsRepository _repository;
        private readonly EmailSendService _emailSendService;
        public NotificationService(
            MachineDownRecordsRepository repository,
            EmailSendService emailSendService)
        { 
            _repository = repository;
            _emailSendService = emailSendService;
        }
        private readonly IEnumerable<string> _from = new[] { "machinedownrecord.center@hella.com" };


        public async Task ScheduleTasksForTomorrowAsync()
        {
            var notifications = _repository.MachineDownRecords.Where(item => !item.MachineRepairedDate.HasValue)
                .Select(item=>new Notification(item,RefreshNotifyDate(item.MachineDownDate)))
                .OrderBy(item=>item.NotifyDate);

            List<Task> notificationTasks = new(notifications.Count());
            foreach(Notification notification in notifications)
            {
                Task task = Task.Run(async () =>
                {
                    await Task.Delay(notification.NotifyDate - DateTimeOffset.Now);
                    await SendNotificationAsync(notification);
                });
                notificationTasks.Add(task);
            }
            await Task.WhenAll(notificationTasks);
                
        }


        public async ValueTask SendNotificationAsync(Notification notification)
        {
            MachineDownRecord record = notification.MachineDownRecord;
            await _repository.Entry(record).Reference(item => item.User).LoadAsync();
            IEnumerable<string> to = new[] { record!.User!.Email };
            string payload = $"[{DateTimeOffset.Now} INF]: {record.EquipmentNo} is down since {record.MachineDownDate}, need to change the state?";
            await _emailSendService.SendEmailAsync(_from, to,
                $"no-reply: machine down record notify at {DateTimeOffset.Now}",
                payload);
        }

        private DateTimeOffset RefreshNotifyDate(DateTimeOffset machineDownDate)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            return new DateTimeOffset(now.Year, now.Month, now.Day + 1, machineDownDate.Hour, machineDownDate.Minute, machineDownDate.Second, now.Offset);
        }
    }
}