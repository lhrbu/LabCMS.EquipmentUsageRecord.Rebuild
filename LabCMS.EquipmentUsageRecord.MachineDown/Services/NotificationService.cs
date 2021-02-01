using System;
using System.Threading.Tasks;
using EasyNetQ;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Services
{
    public class NotificationService
    {
        private readonly MachineDownRecordsRepository _repository;
        private readonly IBus _bus;
        public NotificationService(
            MachineDownRecordsRepository repository,
            IBus bus)
        { 
            _repository = repository;
            _bus = bus;
        }

        public async Task StartMonitorAsync()
        {
            _bus.PubSub.SubscribeAsync<NotificationService>(Guid.NewGuid().ToString(),
                msg=>_);
        }
    }
}