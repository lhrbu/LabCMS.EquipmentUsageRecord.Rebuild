using LabCMS.EquipmentUsageRecord.Shared.Models;
using LabCMS.EquipmentUsageRecord.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class PythonDynamicQueryService
    {
        private readonly IServiceProvider _serviceProvider;
        public PythonDynamicQueryService(IServiceProvider serviceProvider,
            string pythonDll)
        {
            _serviceProvider = serviceProvider;
            Runtime.PythonDLL = pythonDll;

        }

        public object QueryAsync(string code)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            UsageRecordsRepository repository = scope.ServiceProvider.GetRequiredService<UsageRecordsRepository>();
            IEnumerable<UsageRecord> usageRecords = repository.UsageRecords.AsNoTracking();
            using Py.GILState gil = Py.GIL();
            using PyScope pyScope = Py.CreateScope();
            pyScope.Set(nameof(usageRecords), usageRecords.ToPython());
            PyObject pyObject= pyScope.Eval(code);
            if(pyObject.IsIterable())
            {
                return pyObject.Select(item => item.AsManagedObject(typeof(object)));
            }
            else
            {
                object result = pyObject.AsManagedObject(typeof(object));
                return result;
            }
        }
    }
}
