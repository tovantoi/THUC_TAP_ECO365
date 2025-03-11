using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.UserCases.Define.WebLocal;
using _365EJSC.ERP.Application.Validators.Define.WebLocal;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocal
{
    public class GetDetailWebLocalTest
    {
        private readonly Mock<IWebLocalSqlRepository> mockWebLocalSqlRepository;
        private readonly GetDetailWebLocalHandler handler;

        public GetDetailWebLocalTest()
        {
            mockWebLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            handler = new GetDetailWebLocalHandler(mockWebLocalSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnWebLocal_When_Found()
        {
            // Arrange
            var webLocal = new WebLocals
            {
                Id = "AAA",
                Localization = "Web Local 1",
                IsActived = true
            };

            mockWebLocalSqlRepository.Setup(r => r.FindByIdAsync("AAA", false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocal);
            var request = new GetDetailWebLocalRequest { Id = "AAA" };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(webLocal, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_WebLocal_NotFound()
        {
            // Arrange
            mockWebLocalSqlRepository.Setup(r => r.FindByIdAsync("AAA", false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((WebLocals)null);

            var request = new GetDetailWebLocalRequest { Id = "AAA" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_KEY_LOCAL_NOT_FOUND, exception.MessageCode);
        }
    }
}