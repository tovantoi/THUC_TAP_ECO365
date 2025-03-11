using _365EJSC.ERP.Application.Requests.HRM;
using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Application.UserCases.HRM;
using _365EJSC.ERP.Application.UserCases.HRM.TrainingMajor;
using _365EJSC.ERP.Application.Validators.HRM;
using _365EJSC.ERP.Application.Validators.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.TrainingMajor
{
    /// <summary>
    /// Test class for GetTrainingMajorByIdQueryHandler.
    /// </summary>
    public class GetDetailTrainingMajorTest
    {
        private readonly Mock<ITrainingMajorSqlRepository> mockTrainingMajorRepository;
        private readonly GetDetailTrainingMajorHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDetailTrainingMajorTest"/> class.
        /// </summary>
        public GetDetailTrainingMajorTest()
        {
            mockTrainingMajorRepository = new Mock<ITrainingMajorSqlRepository>();
            handler = new GetDetailTrainingMajorHandler(mockTrainingMajorRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnTrainingMajor_When_Found()
        {
            // Arrange
            var TrainingMajor = new Domain.Entities.HRM.TrainingMajor { Id = 1, TmName = "test" };
            mockTrainingMajorRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(TrainingMajor);
            var query = new GetDetailTrainingMajorRequest { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(TrainingMajor, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_TrainingMajor_NotFound()
        {
            // Arrange
            mockTrainingMajorRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_TRAININGMAJOR_ID_NOT_FOUND
                });
            var query = new GetDetailTrainingMajorRequest { Id = 99 };

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<CustomException>(act);
            try
            {
                await handler.Handle(query, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_TRAININGMAJOR_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailTrainingMajorRequest
            {
                // Set invalid properties of GetDetailTrainingMajorQuery here
            };

            var validator = new GetDetailTrainingMajorValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_TRAININGMAJOR_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
