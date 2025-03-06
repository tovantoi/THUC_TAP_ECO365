using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Constants.Define;
using Entities = _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;
using System.Linq.Expressions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalProvince
{
    /// <summary>
    /// Test class for deleting WebLocalProvince entities.
    /// </summary>
    public class DeleteWebLocalProvinceTest
    {
        private readonly Mock<IWebLocalProvinceSqlRepository> mockWebLocalProvinceSqlRepository;
        private readonly Mock<IWebLocalDistrictSqlRepository> mockWebLocalDistrictSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteWebLocalProvinceHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteWebLocalProvinceTest"/> class.
        /// </summary>
        public DeleteWebLocalProvinceTest()
        {
            mockWebLocalProvinceSqlRepository = new Mock<IWebLocalProvinceSqlRepository>();
            mockWebLocalDistrictSqlRepository = new Mock<IWebLocalDistrictSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteWebLocalProvinceHandler(mockWebLocalProvinceSqlRepository.Object, mockWebLocalDistrictSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteWebLocalProvinceRequest { Id = 1 };
            var webLocalProvince = new Domain.Entities.Define.WebLocalProvince();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocalProvince);

            mockWebLocalDistrictSqlRepository
                .Setup(repo => repo.FindSingleAsync(It.IsAny<Expression<Func<Entities.WebLocalDistrict, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Entities.WebLocalDistrict?)null);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockWebLocalProvinceSqlRepository.Verify(repo => repo.Remove(webLocalProvince), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_WebLocalProvinceNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteWebLocalProvinceRequest { Id = 1 };

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_PROVINCE_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PROVINCE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteWebLocalProvinceRequest { Id = 1 };
            var webLocalProvince = new Domain.Entities.Define.WebLocalProvince();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocalProvince);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.Remove(webLocalProvince))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidId_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteWebLocalProvinceRequest { Id = -1 };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PROVINCE_INVALID, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_FoundRelatedDistrict_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteWebLocalProvinceRequest { Id = 1 };
            var webLocalProvince = new Domain.Entities.Define.WebLocalProvince();

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocalProvince);

            mockWebLocalDistrictSqlRepository
                .Setup(repo => repo.FindSingleAsync(It.IsAny<Expression<Func<Entities.WebLocalDistrict, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    CustomException.ThrowNotFoundException(typeof(Entities.WebLocalDistrict), MsgCode.INF_FOUND, WebLocalDistrictConst.MSG_DISTRICT_PROVINCE_EXIST);
                    return new Entities.WebLocalDistrict();
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));

            mockWebLocalProvinceSqlRepository.Verify(repo => repo.Remove(It.IsAny<Domain.Entities.Define.WebLocalProvince>()), Times.Never);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
