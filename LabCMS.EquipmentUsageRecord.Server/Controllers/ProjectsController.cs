using LabCMS.EquipmentUsageRecord.Shared.Models;
using LabCMS.EquipmentUsageRecord.Shared.Repositories;
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

        [HttpGet("Active")]
        public IAsyncEnumerable<Project> GetActiveAsync() =>
            _repository.ActiveProjectIndices.Include(item => item.Project)
                .Select(item => item.Project!).AsNoTracking().AsAsyncEnumerable();

        [HttpPost("Active/{no}")]
        public async ValueTask<IActionResult> PostActiveAsync(string no)
        {
            Project? project = await _repository.Projects.FindAsync(no);
            if(project is not null)
            {
                await _repository.ActiveProjectIndices.AddAsync(new() { No = project.No });
                await _repository.SaveChangesAsync();
                return Ok();
            }
            else { return NotFound($"{no} is not a valid project no."); }
        }

        [HttpDelete("Active/{no}")]
        public async ValueTask<IActionResult> DeleteActiveAsync(string no)
        {
            ActiveProjectIndex? index = await _repository.ActiveProjectIndices.FindAsync(no); 
            if(index is not null)
            {
                _repository.ActiveProjectIndices.Remove(index);
                await _repository.SaveChangesAsync();
                return Ok();
            }
            else { return NotFound($"{no} is not a valid active project no."); }
        }

    }
}
