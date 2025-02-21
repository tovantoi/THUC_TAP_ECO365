using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWard;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWard;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define
{
    public class GetWardByIdTest
    {
        private readonly Mock<IWardSqlRepository> mockWardRepository;
        private readonly GetDetailWardHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSampleTest"/> class.
        /// </summary>
        public GetWardByIdTest()
        {
            mockWardRepository = new Mock<IWardSqlRepository>();
            handler = new GetDetailWardHandler(mockWardRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnSample_When_Found()
        {
            // Arrange
            var sample = new WebsiteLocalizationWard { Id = 1, Name = "Ward 1" };
            mockWardRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);
            var query = new GetDetailWardQuery { Id = 1 };

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
            mockWardRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_WARD_ID_NOT_FOUND
                });
            var query = new GetDetailWardQuery { Id = 99 };

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
                Assert.Equal(MsgCode.ERR_WARD_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailWardQuery
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailWardValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_WARD_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
