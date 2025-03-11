using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralPosition
{
    public class GetErpGeneralPositionByIdTest
    {
        private readonly Mock<IErpGeneralPositionSqlRepository> mockErpGeneralPositionSqlRepository;
        private readonly GetDetailErpGeneralPositionHandler handler;

        public GetErpGeneralPositionByIdTest()
        {
            mockErpGeneralPositionSqlRepository = new Mock<IErpGeneralPositionSqlRepository>();
            handler = new GetDetailErpGeneralPositionHandler(mockErpGeneralPositionSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnErpGeneralPosition_When_Found()
        {
            // Arrange
            var erpGeneralPosition = new Domain.Entities.Define.ErpGeneralPosition
            {
                Id = 1,
                Code = "C01",
                Name = "Test",
            };

            var query = new GetDetailErpGeneralPositionRequest { Id = 1 };
            mockErpGeneralPositionSqlRepository.Setup(r => r.FindByIdAsync((int)query.Id, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(erpGeneralPosition);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_ErpGeneralPosition_NotFound()
        {
            var request = new GetDetailErpGeneralPositionRequest { Id = 99 };

            // Arrange
            mockErpGeneralPositionSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Define.ErpGeneralPosition)null);

            // Act & Assert

        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailErpGeneralPositionRequest
            {
                // 
            };

            var validator = new GetDetailErpGeneralPositionValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_POSITION_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
