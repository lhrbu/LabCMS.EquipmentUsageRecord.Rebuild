using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.Server.Services;
using Python.Runtime;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicQueryController:ControllerBase
    {
        private readonly DynamicQueryService _dynamicQueryService;
        private readonly PythonDynamicQueryService _pythonQuery;
        public DynamicQueryController(
            DynamicQueryService dynamicQueryService,
            PythonDynamicQueryService pythonQuery)
        { 
            _dynamicQueryService = dynamicQueryService;
            _pythonQuery = pythonQuery;
        }

        [HttpPost]
        public dynamic Post([FromBody]string codePiece)=>
            _dynamicQueryService.DynamicQuery(codePiece);
        [HttpPost("Python")]
        public object PostPy([FromBody] string pytcode) =>
            _pythonQuery.QueryAsync(pytcode);
    }
}