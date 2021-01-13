using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class ElasticSearchInteropService:IElasticSearchInteropService
    {
        private readonly ElasticClient _elasticClient;
        public string IndexName { get; } = "usagerecords-raw";
        public ElasticSearchInteropService(
            IConfiguration configuration)
        {
            _elasticClient = new(new Uri(configuration.GetConnectionString("ElasticSearchUrl")));
            _logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.File("es.log")
               .CreateLogger();
            DeclareIndex(IndexName).AsTask().Wait();
        }
        public async ValueTask DeclareIndex(string indexName)
        {
            if(!(await _elasticClient.Indices.ExistsAsync(indexName)).Exists)
            {
                HandleResponse(await _elasticClient.Indices.CreateAsync(indexName,
                    c => c.Map<UsageRecord>(i => i.AutoMap<UsageRecord>())));
            }
        }
        public async ValueTask IndexAsync(UsageRecord usageRecord) =>
             HandleResponse(await _elasticClient.IndexAsync(usageRecord, item => item.Index(IndexName)));

        public async ValueTask IndexManyAsync(IEnumerable<UsageRecord> usageRecords) =>
            HandleResponse(await _elasticClient.IndexManyAsync(usageRecords, IndexName));

        public async ValueTask<IEnumerable<UsageRecord>> SearchAllAsync()
        {
            long size = (await _elasticClient.CountAsync<UsageRecord>(q => q.Index(IndexName))).Count;
            return (await _elasticClient.SearchAsync<UsageRecord>(s => s.Index(IndexName).MatchAll().Size((int)size))).Documents;
        }

        public async ValueTask RemoveByIdAsync(int id)=>
            HandleResponse(await _elasticClient.DeleteAsync<UsageRecord>(id, item => item.Index(IndexName)));
        

        public async ValueTask RemoveManyAsync(IEnumerable<UsageRecord> usageRecords) =>
            HandleResponse(await _elasticClient.DeleteManyAsync(usageRecords, IndexName));
        public async ValueTask RemoveAllAsync() =>
            HandleResponse(await _elasticClient.Indices.DeleteAsync(new DeleteIndexRequest(IndexName)));

        private void HandleResponse(IResponse response)
        {
            if(!response.IsValid)
            {
                _logger.Error("{exception}", response.OriginalException);
                OnException?.Invoke(this, response.OriginalException);
            }
        }

        public event EventHandler<Exception>? OnException;
        private readonly Logger _logger;
    }
}
