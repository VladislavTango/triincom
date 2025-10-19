using AutoMapper;
using FluentAssertions;
using Moq;
using triincom.Application.Services;
using triincom.Core.DTO;
using triincom.Core.Entities;
using triincom.Core.Enums;
using triincom.Core.Interface;

namespace triincom.Tests.Services
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IApplicationRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ApplicationService _service;

        public ApplicationServiceTests()
        {
            _repositoryMock = new Mock<IApplicationRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ApplicationService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddAplication_WithValidData_ShouldCallRepository()
        {
            // Arrange
            var applicationDto = new AddApplicationDto
            {
                InterestValue = 5.0m,
                TermValue = 12,
                Amount = 10000m
            };

            var applicationEntity = new ApplicationEntity();
            var savedEntity = new ApplicationEntity();

            _mapperMock.Setup(m => m.Map<ApplicationEntity>(applicationDto)).Returns(applicationEntity);
            _repositoryMock.Setup(r => r.AddAsync(applicationEntity)).ReturnsAsync(savedEntity);

            // Act
            await _service.AddAplication(applicationDto);

            // Assert
            _mapperMock.Verify(m => m.Map<ApplicationEntity>(applicationDto), Times.Once);
            _repositoryMock.Verify(r => r.AddAsync(applicationEntity), Times.Once);
        }

        [Fact]
        public async Task AddAplication_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            var applicationDto = new AddApplicationDto
            {
                InterestValue = 0,
                TermValue = 12,
                Amount = 10000m
            };

            // Act
            Func<Task> act = async () => await _service.AddAplication(applicationDto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("не все данные заполнены");
            _mapperMock.Verify(m => m.Map<ApplicationEntity>(It.IsAny<AddApplicationDto>()), Times.Never);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ApplicationEntity>()), Times.Never);
        }

        [Fact]
        public async Task ChangeApplicationStatus_ShouldCallRepository()
        {
            // Arrange
            var statusDto = new ChangeStatusDto
            {
                Number = "APP-001",
                Status = Status.Published
            };

            _repositoryMock.Setup(r => r.ChangeStatus(statusDto)).Returns(Task.CompletedTask);

            // Act
            await _service.ChangeApplicationStatus(statusDto);

            // Assert
            _repositoryMock.Verify(r => r.ChangeStatus(statusDto), Times.Once);
        }

        [Fact]
        public async Task GetAllApplicationsAsync_ShouldReturnDataFromRepository()
        {
            // Arrange
            var expectedResult = new PaginatedResult<ApplicationEntity>
            {
                Data = new List<ApplicationEntity>
                {
                    new ApplicationEntity {
                        Id = Guid.NewGuid(),
                        Number = "APP-001"
                    }
                },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _repositoryMock.Setup(r => r.GetFilteredAsync(null)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetAllApplicationsAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _repositoryMock.Verify(r => r.GetFilteredAsync(null), Times.Once);
        }

        [Fact]
        public async Task GetFilteredApplicationsAsync_ShouldReturnFilteredData()
        {
            // Arrange
            var filter = new ApplicationFilterDto { Status = Status.Published };
            var expectedResult = new PaginatedResult<ApplicationEntity>
            {
                Data = new List<ApplicationEntity>
                {
                    new ApplicationEntity { 
                        Id = Guid.NewGuid(),
                        Number = "APP-001",
                        Status = Status.Published
                    }
                },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _repositoryMock.Setup(r => r.GetFilteredAsync(filter)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetFilteredApplicationsAsync(filter);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _repositoryMock.Verify(r => r.GetFilteredAsync(filter), Times.Once);
        }
    }
}
