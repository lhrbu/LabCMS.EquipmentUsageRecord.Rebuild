﻿using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using LabCMS.EquipmentUsageRecord.Server.Services;
using LabCMS.EquipmentUsageRecord.Server.Repositories;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentHourlyRatesController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        public EquipmentHourlyRatesController(
            UsageRecordsRepository repository)
        { _repository = repository; }
        [HttpGet]
        public IEnumerable<EquipmentHourlyRate> Get() =>
            _repository.EquipmentHourlyRates.AsNoTracking();
    }
}
