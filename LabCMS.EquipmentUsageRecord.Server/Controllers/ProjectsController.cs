using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly UsageRecordsRepository _repository;
        public ProjectsController(
            UsageRecordsRepository repository)
        { _repository = repository; }

        [HttpGet]
        public IAsyncEnumerable<Project> GetAsync() =>
            _repository.Projects.AsNoTracking().AsAsyncEnumerable();
        [HttpPost]
        public async ValueTask PostAsync(Project project)
        {
            await _repository.Projects.AddAsync(project);
            await _repository.SaveChangesAsync();
        }
        [HttpPut]
        public async ValueTask PutAsync(Project project)
        {
            _repository.Projects.Update(project);
            await _repository.SaveChangesAsync();
        }

        [HttpDelete]
        public async ValueTask DeleteByNo(string projectNo)
        {
            Project? project = await _repository.Projects.FindAsync(projectNo);
            if(project is not null)
            {
                await _repository.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                if(!await _repository.UsageRecords.AnyAsync(item=>item.ProjectNo==projectNo))
                {
                    _repository.Projects.Remove(project);
                }
                await _repository.Database.CommitTransactionAsync();
            }
        }
    }
}
