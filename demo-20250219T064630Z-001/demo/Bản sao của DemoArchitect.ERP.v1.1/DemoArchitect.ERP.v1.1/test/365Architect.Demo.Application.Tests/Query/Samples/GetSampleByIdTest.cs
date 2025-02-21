using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Application.UserCases.Samples;
using _365Architect.Demo.Application.Validators.Samples;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Domain.Abstractions.Repositories.Sql;
using _365Architect.Demo.Domain.Entities;
using Moq;

namespace _365Architect.Demo.Application.Tests.Query.Samples
{
    /// <summary>
    /// Test class for GetSampleByIdQueryHandler.
    /// </summary>
    public class GetSampleByIdTest
    {
        private readonly Mock<ISampleSqlRepository> mockSampleRepository;
        private readonly GetDetailSampleHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSampleByIdTest"/> class.
        /// </summary>
        public GetSampleByIdTest()
        {
            mockSampleRepository = new Mock<ISampleSqlRepository>();
            handler = new GetDetailSampleHandler(mockSampleRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSample_When_Found()
        {
            // Arrange
            var sample = new Sample { Id = 1, Title = "Sample 1" };
            mockSampleRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);
            var query = new GetDetailSampleQuery { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(sample, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_Sample_NotFound()
        {
            // Arrange
            mockSampleRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_SAMPLE_ID_NOT_FOUND
                });
            var query = new GetDetailSampleQuery { Id = 99 };

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
                Assert.Equal(MsgCode.ERR_SAMPLE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailSampleQuery
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailSampleValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_SAMPLE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}