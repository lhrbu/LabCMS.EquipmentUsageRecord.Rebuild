using EasyNetQ;
using LabCMS.EquipmentUsageRecord.Shared.Events;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using LabCMS.EquipmentUsageRecord.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.DataPersistenceService
{
    public class UsageRecordDataPersisentService:BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _lifetime;
        public UsageRecordDataPersisentService(IServiceProvider serviceProvider,
            IHostApplicationLifetime lifetime)
        { 
            _serviceProvider = serviceProvider;
            _lifetime = lifetime;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Console.WriteLine("Start Listenning!");
                IBus bus = _serviceProvider.GetRequiredService<IBus>();
                bus.PubSub.SubscribeAsync<UsageRecordPersisientEventArgs>(
                    nameof(MessageHandleServices.DataPersistenceService),
                    eventArgs => Task.Run( async()=>
                    {
                        //using IServiceScope scope = _serviceProvider.CreateScope();
                        //UsageRecordsRepository repository = scope.ServiceProvider.GetRequiredService<UsageRecordsRepository>();
                        //_ = eventArgs.EventKind switch
                        //{
                        //    UsageRecordPersisientEventKind.Add => await repository.UsageRecords.AddAsync(eventArgs.UsageRecord),
                        //    UsageRecordPersisientEventKind.Update => repository.UsageRecords.Update(eventArgs.UsageRecord),
                        //    UsageRecordPersisientEventKind.Delete => repository.UsageRecords.Remove(eventArgs.UsageRecord),
                        //    _ => throw new ArgumentException("Invalid Event Kind",nameof(eventArgs.EventKind)),
                        //};
                        //await repository.SaveChangesAsync();
                    }).ContinueWith(task=>
                    {
                        if(task.IsFaulted && task.Exception!=null)
                        { Console.WriteLine(task.Exception); }
                    }));
                Console.WriteLine("In running, Press Y to stop the service!");
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(true);
                } while (keyInfo.Key != ConsoleKey.Y);
                return Task.CompletedTask;
            }
            finally
            {
                _lifetime.StopApplication();
            }
        }
    }
}
