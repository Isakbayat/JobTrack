using JobTrack.Api.Data;
using JobTrack.Api.Dtos;
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
        public async Task<ActionResult<JobApplication>> Create(
            JobApplicationRequest request
        )
        {
            var application = new JobApplication
            {
                Company = request.Company,
                Position = request.Position,
                AppliedDate = request.AppliedDate,
                Status = request.Status
            };

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
            JobApplicationRequest request
        )
        {
            var application = await _context.JobApplications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            application.Company = request.Company;
            application.Position = request.Position;
            application.AppliedDate = request.AppliedDate;
            application.Status = request.Status;

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