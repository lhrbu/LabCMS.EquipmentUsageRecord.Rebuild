using LabCMS.EquipmentUsageRecord.Server.Repositories;
using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raccoon.Devkits.JwtAuthroization.Models;
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
        [CookieJwtPayloadRequirement("usage_records","role","admin")]
        public async ValueTask PostAsync(Project project)
        {
            await _repository.Projects.AddAsync(project);
            await _repository.SaveChangesAsync();
        }
        [HttpPut]
        [CookieJwtPayloadRequirement("usage_records", "role", "admin")]
        public async ValueTask PutAsync(Project project)
        {
            _repository.Projects.Update(project);
            await _repository.SaveChangesAsync();
        }

        [HttpDelete("{projectName}")]
        [CookieJwtPayloadRequirement("usage_records", "role", "admin")]
        public async ValueTask<IActionResult> DeleteByName(string projectName)
        {
            string decodedProjectName = System.Web.HttpUtility.UrlDecode(projectName);
            Project? project = await _repository.Projects.FirstOrDefaultAsync(item => item.Name == decodedProjectName);
            if(project is not null)
            {
                await _repository.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                if(!await _repository.UsageRecords.AnyAsync(item=>item.ProjectNo==project.No))
                {
                    _repository.Projects.Remove(project);
                }
                await _repository.Database.CommitTransactionAsync();
                await _repository.SaveChangesAsync();
                return Ok();
            }
            else { return NotFound(); }
        }
    }
}
