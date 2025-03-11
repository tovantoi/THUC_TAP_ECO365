using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompanyPosition
{
    public class GetDetailCompanyPositionTest
    {
        private readonly Mock<ICompanyPositionSqlRepository> mockCompanyPositionRepository;
        private readonly GetDetailCompanyPositionHandler handler;

        public GetDetailCompanyPositionTest()
        {
            mockCompanyPositionRepository = new Mock<ICompanyPositionSqlRepository>();
            handler = new GetDetailCompanyPositionHandler(mockCompanyPositionRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnCompanyPosition_When_Found()
        {
            // Arrange
            var companyPosition = new Entities.ErpGeneralCompanyPosition
            {
                Id = 1,
                CompanyId = 1,
                PositionId = 1
            };

            mockCompanyPositionRepository
                .Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyPosition);

            var request = new GetDetailCompanyPositionRequest { Id = 1 };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyPosition, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_CompanyPosition_NotFound()
        {
            // Arrange
            mockCompanyPositionRepository
                .Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Entities.ErpGeneralCompanyPosition)null);

            var request = new GetDetailCompanyPositionRequest { Id = 99 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_COMPANY_POSITION_ID_NOT_FOUND, exception.MessageCode);
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailCompanyPositionRequest();

            var validator = new GetDetailCompanyPositionValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_COMPANY_POSITION_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}