using System.ComponentModel.DataAnnotations;
using JobTrack.Api.Controllers;
using JobTrack.Api.Data;
using JobTrack.Api.Dtos;
using JobTrack.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTrack.Api.Tests
{
    public class ApplicationsControllerTests
    {
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Create_AddsApplicationToDatabase()
        {
            await using var context = CreateContext();
            var controller = new ApplicationsController(context);

            var request = new JobApplicationRequest
            {
                Company = "Telenor",
                Position = "System Developer Intern",
                AppliedDate = new DateTime(2026, 10, 7),
                Status = "Applied"
            };

            var result = await controller.Create(request);

            var createdResult =
                Assert.IsType<CreatedAtActionResult>(result.Result);

            Assert.Equal(
                nameof(ApplicationsController.GetById),
                createdResult.ActionName
            );

            Assert.Equal(1, await context.JobApplications.CountAsync());

            var savedApplication =
                await context.JobApplications.SingleAsync();

            Assert.Equal("Telenor", savedApplication.Company);
            Assert.Equal(
                "System Developer Intern",
                savedApplication.Position
            );
            Assert.Equal("Applied", savedApplication.Status);
        }

        [Fact]
        public async Task GetAll_ReturnsApplications()
        {
            await using var context = CreateContext();

            context.JobApplications.AddRange(
                new JobApplication
                {
                    Company = "Telenor",
                    Position = "System Developer Intern",
                    AppliedDate = new DateTime(2026, 10, 7),
                    Status = "Applied"
                },
                new JobApplication
                {
                    Company = "SEB",
                    Position = ".NET Developer Intern",
                    AppliedDate = new DateTime(2026, 10, 6),
                    Status = "Interview"
                }
            );

            await context.SaveChangesAsync();

            var controller = new ApplicationsController(context);

            var result = await controller.GetAll();

            var okResult =
                Assert.IsType<OkObjectResult>(result.Result);

            var applications =
                Assert.IsAssignableFrom<IEnumerable<JobApplication>>(
                    okResult.Value
                );

            Assert.Equal(2, applications.Count());
        }

        [Fact]
        public async Task Update_ChangesExistingApplication()
        {
            await using var context = CreateContext();

            var application = new JobApplication
            {
                Company = "Telenor",
                Position = "System Developer Intern",
                AppliedDate = new DateTime(2026, 10, 7),
                Status = "Applied"
            };

            context.JobApplications.Add(application);
            await context.SaveChangesAsync();

            var controller = new ApplicationsController(context);

            var request = new JobApplicationRequest
            {
                Company = "Telenor",
                Position = "Junior System Developer",
                AppliedDate = new DateTime(2026, 10, 7),
                Status = "Offer"
            };

            var result = await controller.Update(
                application.Id,
                request
            );

            Assert.IsType<NoContentResult>(result);

            context.ChangeTracker.Clear();

            var savedApplication =
                await context.JobApplications.FindAsync(application.Id);

            Assert.NotNull(savedApplication);
            Assert.Equal(
                "Junior System Developer",
                savedApplication.Position
            );
            Assert.Equal("Offer", savedApplication.Status);
        }

        [Fact]
        public async Task Delete_RemovesApplication()
        {
            await using var context = CreateContext();

            var application = new JobApplication
            {
                Company = "SEB",
                Position = ".NET Developer Intern",
                AppliedDate = new DateTime(2026, 10, 6),
                Status = "Applied"
            };

            context.JobApplications.Add(application);
            await context.SaveChangesAsync();

            var controller = new ApplicationsController(context);

            var result = await controller.Delete(application.Id);

            Assert.IsType<NoContentResult>(result);

            Assert.Equal(
                0,
                await context.JobApplications.CountAsync()
            );
        }

        [Fact]
        public void JobApplicationRequest_WithoutCompany_IsInvalid()
        {
            var request = new JobApplicationRequest
            {
                Company = "",
                Position = "System Developer Intern",
                AppliedDate = new DateTime(2026, 10, 7),
                Status = "Applied"
            };

            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(
                request,
                new ValidationContext(request),
                validationResults,
                true
            );

            Assert.False(isValid);

            Assert.Contains(
                validationResults,
                result => result.MemberNames.Contains(nameof(request.Company))
            );
        }

        [Fact]
        public void JobApplicationRequest_WithInvalidStatus_IsInvalid()
        {
            var request = new JobApplicationRequest
            {
                Company = "Telenor",
                Position = "System Developer Intern",
                AppliedDate = new DateTime(2026, 10, 7),
                Status = "Unknown"
            };

            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(
                request,
                new ValidationContext(request),
                validationResults,
                true
            );

            Assert.False(isValid);

            Assert.Contains(
                validationResults,
                result => result.MemberNames.Contains(nameof(request.Status))
            );
        }
    }
}