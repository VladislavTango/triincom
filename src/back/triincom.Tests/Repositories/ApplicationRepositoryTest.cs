using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using triincom.Core.DTO;
using triincom.Core.Entities;
using triincom.Core.Enums;
using triincom.DataPersistence.AppContext;
using triincom.Infrastructure.Repositories;

namespace triincom.Tests.Repositories
{
    public class ApplicationRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ApplicationRepository _repository;

        public ApplicationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new ApplicationRepository(_context);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        [Fact]
        public async Task AddAsync_WithValidEntity_ShouldSetPropertiesAndSaveToDatabase()
        {
            // Arrange
            var application = new ApplicationEntity
            {
                Amount = 10000m,
                TermValue = 12,
                InterestValue = 5.0m
            };

            // Act
            var result = await _repository.AddAsync(application);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(Status.Published);
            result.Number.Should().NotBeNullOrEmpty();
            result.CreatedAt.Should().NotBe(default);
            result.ModifiedAt.Should().NotBe(default);
        }

        [Fact]
        public async Task ChangeStatus_WithExistingNumber_ShouldUpdateStatus()
        {
            // Arrange
            var existingApp = new ApplicationEntity
            {
                Amount = 10000m,
                TermValue = 12,
                InterestValue = 5.0m,
                Status = Status.Published,
                Number = "TEST-001"
            };
            _context.Applications.Add(existingApp);
            await _context.SaveChangesAsync();

            var statusDto = new ChangeStatusDto
            {
                Number = "TEST-001",
                Status = Status.Unpublished
            };

            // Act
            await _repository.ChangeStatus(statusDto);

            // Assert
            var updatedApp = await _context.Applications.FirstAsync(x => x.Number == "TEST-001");
            updatedApp.Status.Should().Be(Status.Unpublished);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingId_ShouldReturnEntity()
        {
            // Arrange
            var expectedApp = new ApplicationEntity
            {
                Id = Guid.NewGuid(),
                Amount = 10000m,
                TermValue = 12,
                InterestValue = 5.0m,
                Status = Status.Published,
                Number = "TEST-001"
            };
            _context.Applications.Add(expectedApp);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(expectedApp.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedApp.Id);
        }

        [Fact]
        public async Task GetByNumberAsync_WithExistingNumber_ShouldReturnEntity()
        {
            // Arrange
            var expectedApp = new ApplicationEntity
            {
                Amount = 10000m,
                TermValue = 12,
                InterestValue = 5.0m,
                Status = Status.Published,
                Number = "TEST-001"
            };
            _context.Applications.Add(expectedApp);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByNumberAsync("TEST-001");

            // Assert
            result.Should().NotBeNull();
            result.Number.Should().Be("TEST-001");
        }

        [Fact]
        public async Task GetFilteredAsync_WithNullFilter_ShouldReturnAllApplications()
        {
            // Arrange
            var testData = new List<ApplicationEntity>
            {
                new() { Amount = 5000m, TermValue = 6, InterestValue = 4.0m, Status = Status.Published, Number = "APP-001" },
                new() { Amount = 15000m, TermValue = 12, InterestValue = 5.0m, Status = Status.Published, Number = "APP-002" }
            };
            _context.Applications.AddRange(testData);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFilteredAsync(null);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetFilteredAsync_WithPagination_ShouldReturnPagedResults()
        {
            // Arrange
            var testData = new List<ApplicationEntity>
            {
                new() { Amount = 5000m, TermValue = 6, InterestValue = 4.0m, Status = Status.Published, Number = "APP-001" },
                new() { Amount = 15000m, TermValue = 12, InterestValue = 5.0m, Status = Status.Published, Number = "APP-002" },
                new() { Amount = 25000m, TermValue = 24, InterestValue = 6.0m, Status = Status.Published, Number = "APP-003" }
            };
            _context.Applications.AddRange(testData);
            await _context.SaveChangesAsync();

            var filter = new ApplicationFilterDto { PageNumber = 1, PageSize = 2 };

            // Act
            var result = await _repository.GetFilteredAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.TotalCount.Should().Be(3);
            result.HasNext.Should().BeTrue();
        }

        [Fact]
        public async Task GetFilteredAsync_WithStatusFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var testData = new List<ApplicationEntity>
            {
                new() { Amount = 5000m, TermValue = 6, InterestValue = 4.0m, Status = Status.Published, Number = "APP-001" },
                new() { Amount = 15000m, TermValue = 12, InterestValue = 5.0m, Status = Status.Unpublished, Number = "APP-002" }
            };
            _context.Applications.AddRange(testData);
            await _context.SaveChangesAsync();

            var filter = new ApplicationFilterDto { Status = Status.Published };

            // Act
            var result = await _repository.GetFilteredAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.Data.First().Status.Should().Be(Status.Published);
        }
    }
}
