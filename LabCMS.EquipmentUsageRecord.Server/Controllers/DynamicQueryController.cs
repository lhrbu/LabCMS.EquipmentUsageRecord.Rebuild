using LabCMS.EquipmentUsageRecord.Server.Repositories;
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

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicQueryController:ControllerBase
    {
        private readonly DynamicQueryService _dynamicQueryService;
        public DynamicQueryController(DynamicQueryService dynamicQueryService)
        { _dynamicQueryService = dynamicQueryService;}

        [HttpPost]
        public dynamic Post([FromBody]string codePiece)=>
            _dynamicQueryService.DynamicQuery(codePiece);
    }
}