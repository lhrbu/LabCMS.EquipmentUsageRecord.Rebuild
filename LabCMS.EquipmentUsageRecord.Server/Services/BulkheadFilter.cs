using Microsoft.AspNetCore.Mvc.Filters;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class BulkheadFilter : IAsyncResultFilter
    {
        private IReadOnlyPolicyRegistry<string> _policyRegistry;
        public BulkheadFilter(IReadOnlyPolicyRegistry<string> policyRegistry)
        { _policyRegistry = policyRegistry; }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if(_policyRegistry.TryGet(nameof(BulkheadFilter),out AsyncPolicy? policy) && policy is not null)
            {
                await policy.ExecuteAsync(async () => await next());
            }
            else
            {
                throw new InvalidOperationException($"{nameof(BulkheadFilter)} doesn't register Bulkhead policy!");
            }
            
        }
    }
}
