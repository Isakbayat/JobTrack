using JobTrack.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private static readonly List<JobApplication> applications = new List<JobApplication>
        {
            new JobApplication
            {
                Id = 1,
                Company = "Telenor",
                Position = "System Developer Intern",
                AppliedDate = new DateTime(2026, 9, 9),
                Status = "Applied"
            },

            new JobApplication
            {
                Id = 2,
                Company = "SEB",
                Position = ".NET Developer Intern",
                AppliedDate = new DateTime(2026, 9, 8),
                Status = "Interview"
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<JobApplication>> GetAll()
        {
            return Ok(applications);
        }

        [HttpGet("{id}")]
        public ActionResult<JobApplication> GetById(int id)
        {
            var application = applications.FirstOrDefault(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }
    }
}