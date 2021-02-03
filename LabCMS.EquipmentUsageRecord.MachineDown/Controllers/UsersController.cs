using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.MachineDown.Repositories;
using LabCMS.EquipmentUsageRecord.MachineDown.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        private readonly MachineDownRecordsRepository _repository;
        public UsersController(MachineDownRecordsRepository repository)
        { _repository=repository;}

        [HttpGet]
        public IAsyncEnumerable<User> GetAsync()=>_repository.Users.AsAsyncEnumerable();
    }
}