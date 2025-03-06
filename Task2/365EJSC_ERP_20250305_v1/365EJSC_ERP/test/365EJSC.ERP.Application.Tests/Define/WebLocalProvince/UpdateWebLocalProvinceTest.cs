using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalProvince
{
    /// <summary>
    /// Test class for updating WebLocalProvince entities.
    /// </summary>
    public class UpdateWebLocalProvinceTest
    {
        private readonly Mock<IWebLocalProvinceSqlRepository> mockWebLocalProvinceSqlRepository;
        private readonly Mock<IWebLocalSqlRepository> mockWebLocalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateWebLocalProvinceHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateWebLocalProvinceTest"/> class.
        /// </summary>
        public UpdateWebLocalProvinceTest()
        {
            mockWebLocalProvinceSqlRepository = new Mock<IWebLocalProvinceSqlRepository>();
            mockWebLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateWebLocalProvinceHandler(mockWebLocalProvinceSqlRepository.Object, mockWebLocalSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new UpdateWebLocalProvinceRequest { Id = 1 };
            var webLocalProvince = new Domain.Entities.Define.WebLocalProvince();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocalProvince);

            Expression<Func<WebLocals, bool>> predicate = x => (x.Id == request.KeyLocalization);
            mockWebLocalSqlRepository
                .Setup(repo => repo.IsExistAsync(predicate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockWebLocalProvinceSqlRepository.Verify(repo => repo.Update(webLocalProvince), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_WebLocalProvinceNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateWebLocalProvinceRequest { Id = 1 };

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
            var request = new UpdateWebLocalProvinceRequest { Id = 1 };
            var webLocalProvince = new Domain.Entities.Define.WebLocalProvince();
            var mockTransaction = new Mock<IDbTransaction>();

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocalProvince);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.Update(webLocalProvince))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new UpdateWebLocalProvinceRequest { Id = 0 };
            var validator = new UpdateWebLocalProvinceValidator();

            // Act & Assert
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PROVINCE_INVALID, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_KeyLocalizationNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateWebLocalProvinceRequest { Id = 1, KeyLocalization = "lao" };
            var webLocalProvince = new Domain.Entities.Define.WebLocalProvince { Id = 1, KeyLocalization = "vietname" };

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocalProvince);

            mockWebLocalSqlRepository
                .Setup(repo => repo.IsExistAsync(It.IsAny<Expression<Func<WebLocals, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    CustomException.ThrowNotFoundException(typeof(WebLocals), MsgCode.ERR_KEY_LOCAL_NOT_FOUND, WebLocalConst.MSG_KEY_LOCALIZATION_NOT_FOUND);
                    return false;
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));

            mockWebLocalProvinceSqlRepository.Verify(repo => repo.Update(It.IsAny<Domain.Entities.Define.WebLocalProvince>()), Times.Never);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
