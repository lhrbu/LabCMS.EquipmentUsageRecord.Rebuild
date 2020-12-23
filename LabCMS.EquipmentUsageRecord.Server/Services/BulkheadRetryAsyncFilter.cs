using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Bulkhead;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class BulkheadRetryAsyncFilter : IAsyncResultFilter
    {
        private IReadOnlyPolicyRegistry<string> _policyRegistry;
        public BulkheadRetryAsyncFilter(IReadOnlyPolicyRegistry<string> policyRegistry)
        { _policyRegistry = policyRegistry; }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if(_policyRegistry.TryGet(nameof(BulkheadRetryAsyncFilter),out AsyncPolicy? policy) && policy is not null)
            {
                await policy.ExecuteAsync(async () => await next());
            }
            else
            {
                throw new InvalidOperationException($"{nameof(BulkheadRetryAsyncFilter)} doesn't register Bulkhead policy!");
            }
        }
    }

    public static class BulkheadRetryAsyncFilterExtensions
    {
        public static IServiceCollection AddBulkheadRetryAsyncFilter(this IServiceCollection services)
        {
            services.TryAddSingleton<BulkheadRetryAsyncFilter>();
            PolicyRegistry policyRegistry = new()
            {
                {
                    nameof(BulkheadRetryAsyncFilter),
                    Policy.WrapAsync(
                        Policy.BulkheadAsync(128, 4),
                        Policy.Handle<BulkheadRejectedException>().WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                        )
                }
            };
            services.TryAddSingleton<IReadOnlyPolicyRegistry<string>>(policyRegistry);
            return services;
        }
    }
}
