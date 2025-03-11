using _365EJSC.ERP.Application.Requests.HRM;
using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Application.UserCases.HRM;
using _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.TrainingMajor
{
    public class GetAllTrainingMajorTest
    {
        private readonly Mock<ITrainingMajorSqlRepository> mockGeneralDepartmentRepository;
        private readonly GetAllTrainingMajorHandler handler;

        public GetAllTrainingMajorTest()
        {
            mockGeneralDepartmentRepository = new Mock<ITrainingMajorSqlRepository>();
            handler = new GetAllTrainingMajorHandler(mockGeneralDepartmentRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoTrainingMajor()
        {
            // Arrange
            mockGeneralDepartmentRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Domain.Entities.HRM.TrainingMajor>().AsQueryable());
            var query = new GetAllTrainingMajorRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllTrainingMajor()
        {
            // Arrange
            var TrainingMajor = new List<Domain.Entities.HRM.TrainingMajor>
        {
            new() { Id = 1, TmName = "test" },
            new() { Id = 2, TmName = "test" }
        };
            mockGeneralDepartmentRepository.Setup(repository => repository.FindAll(null, false)).Returns(TrainingMajor.AsQueryable());
            var query = new GetAllTrainingMajorRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count);
        }
    }
}
