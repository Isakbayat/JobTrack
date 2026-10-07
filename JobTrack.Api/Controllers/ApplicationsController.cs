using JobTrack.Api.Data;
using JobTrack.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplication>>> GetAll()
        {
            var applications = await _context.JobApplications.ToListAsync();

            return Ok(applications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplication>> GetById(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }

        [HttpPost]
        public async Task<ActionResult<JobApplication>> Create(JobApplication application)
        {
            _context.JobApplications.Add(application);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = application.Id },
                application
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            JobApplication updatedApplication
        )
        {
            var application = await _context.JobApplications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            application.Company = updatedApplication.Company;
            application.Position = updatedApplication.Position;
            application.AppliedDate = updatedApplication.AppliedDate;
            application.Status = updatedApplication.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            _context.JobApplications.Remove(application);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}