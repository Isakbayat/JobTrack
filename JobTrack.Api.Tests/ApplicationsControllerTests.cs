using JobTrack.Api.Controllers;
using JobTrack.Api.Data;
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
            // Arrange
            await using var context = CreateContext();
            var controller = new ApplicationsController(context);

            var application = new JobApplication
            {
                Company = "Telenor",
                Position = "System Developer Intern",
                AppliedDate = new DateTime(2026, 10, 7),
                Status = "Applied"
            };

            // Act
            var result = await controller.Create(application);

            // Assert
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
            // Arrange
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

            // Act
            var result = await controller.GetAll();

            // Assert
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
            // Arrange
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

            var updatedApplication = new JobApplication
            {
                Company = "Telenor",
                Position = "Junior System Developer",
                AppliedDate = new DateTime(2026, 10, 7),
                Status = "Offer"
            };

            // Act
            var result = await controller.Update(
                application.Id,
                updatedApplication
            );

            // Assert
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
            // Arrange
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

            // Act
            var result = await controller.Delete(application.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            Assert.Equal(
                0,
                await context.JobApplications.CountAsync()
            );
        }
    }
}