using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.UserCases.HRM.Marital;
using _365EJSC.ERP.Application.Validators.HRM.Marital;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.Marital
{
    public class GetDetailMaritalTest
    {
        private readonly Mock<IMaritalSqlRepository> mockMaritalSqlRepository;
        private readonly GetDetailMaritalHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSampleByIdTest"/> class.
        /// </summary>
        public GetDetailMaritalTest()
        {
            mockMaritalSqlRepository = new Mock<IMaritalSqlRepository>();
            handler = new GetDetailMaritalHandler(mockMaritalSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnSample_When_Found()
        {
            // Arrange
            var sample = new HrmMarital { Id = 1, Name = "Sample 1" };
            mockMaritalSqlRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);
            var query = new GetDetailMaritalRequest { Id = 1 };

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
            mockMaritalSqlRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_MARITAL_ID_NOT_FOUND
                });
            var query = new GetDetailMaritalRequest { Id = 99 };

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
                Assert.Equal(MsgCode.ERR_MARITAL_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailMaritalRequest
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailMaritalValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_MARITAL_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}

