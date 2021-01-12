using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using LabCMS.EquipmentUsageRecord.Server.Repositories;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class DynamicQueryService
    {
        private readonly UsageRecordsRepository  _usageRecordsRepository;
        public DynamicQueryService(UsageRecordsRepository usageRecordsRepository)
        { _usageRecordsRepository=usageRecordsRepository;}

        private readonly CSharpCompilationOptions _compilationOptions = new(
            outputKind:OutputKind.DynamicallyLinkedLibrary,
            optimizationLevel:OptimizationLevel.Release);
        
        private Assembly Compile(string assemblyName,string code,Assembly[] referenceAssemblies)
        {
            IEnumerable<PortableExecutableReference> _references = referenceAssemblies
                .Select(assembly=>MetadataReference.CreateFromFile(assembly.Location));
            CSharpCompilation cSharpCompilation = CSharpCompilation.Create(assemblyName)
                .WithReferences(_references).WithOptions(_compilationOptions)
			    .AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));
            using MemoryStream memoryStream = new ();
		    cSharpCompilation.Emit(memoryStream);
		    memoryStream.Seek(0L, SeekOrigin.Begin);
            return Assembly.Load(memoryStream.ToArray());
        }

        public dynamic DynamicQuery(string codePiece)
        {
            string assemblyId =Guid.NewGuid().ToString().Replace('-','_');
            string code =
$@"
using LabCMS.EquipmentDomain.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentDomain.Temp_{assemblyId}
{{
    public class DynamicQuereInstance
    {{
        public dynamic DynamicQuery(IEnumerable<UsageRecord> usageRecords)
        {{
            {codePiece}
        }}
    }}
}}
";
            Assembly tempAssembly = Compile($"AssemblyTemp{assemblyId}",code,
                AssemblyLoadContext.Default.Assemblies.Where(assembly=>!assembly.IsDynamic).ToArray());
            Type instanceType = tempAssembly.GetType($"LabCMS.EquipmentDomain.Temp_{assemblyId}.DynamicQuereInstance")!;
            dynamic instance = Activator.CreateInstance(instanceType)!;
            dynamic result = instance.DynamicQuery(_usageRecordsRepository.UsageRecords
                .Include(item=>item.Project)
                .Include(item=>item.EquipmentHourlyRate)
                .AsNoTracking());
            return result;
        }
    }
}