using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class DbLogHandleService:IDisposable
    {
        private readonly StreamWriter _logWriter = new("db.log", append: true) { AutoFlush = true };
        private readonly Channel<string> _queue = Channel.CreateUnbounded<string>();
        private readonly CancellationTokenSource _tokenSource = new();
        public async Task BeginWriteDbLog()
        {
            while(await _queue.Reader.WaitToReadAsync(_tokenSource.Token))
            {
                string log = await _queue.Reader.ReadAsync(_tokenSource.Token);
                _logWriter.WriteLine(log);
            }
        }

        public void Dispose()
        {
            _tokenSource.Token.Register(_logWriter.Dispose);
            _tokenSource.Cancel();
            GC.SuppressFinalize(this);
        }

        public void EnqueueDbLog(string log)=>_queue.Writer.TryWrite(log);

    }
}
