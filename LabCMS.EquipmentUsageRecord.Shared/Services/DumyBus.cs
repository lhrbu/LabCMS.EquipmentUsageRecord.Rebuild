using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.Services
{
    public class DumyBus : IBus
    {
        public IPubSub PubSub => throw new NotImplementedException();

        public IRpc Rpc => throw new NotImplementedException();

        public ISendReceive SendReceive => throw new NotImplementedException();

        public IScheduler Scheduler => throw new NotImplementedException();

        public IAdvancedBus Advanced => throw new NotImplementedException();

        public void Dispose()
        {

        }
    }
}
