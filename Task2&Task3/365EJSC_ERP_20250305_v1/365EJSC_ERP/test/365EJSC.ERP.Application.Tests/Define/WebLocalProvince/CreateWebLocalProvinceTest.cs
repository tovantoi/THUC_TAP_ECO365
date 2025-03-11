using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Constants.Define;
using Entities = _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalProvince
{
    /// <summary>
    /// Test class for creating WebLocalProvince entities.
    /// </summary>
    public class CreateWebLocalProvinceTest
    {
        private readonly Mock<IWebLocalProvinceSqlRepository> mockWebLocalProvinceSqlRepository;
        private readonly Mock<IWebLocalSqlRepository> mockWebLocalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateWebLocalProvinceHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateWebLocalProvinceTest"/> class.
        /// </summary>
        public CreateWebLocalProvinceTest()
        {
            mockWebLocalProvinceSqlRepository = new Mock<IWebLocalProvinceSqlRepository>();
            mockWebLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateWebLocalProvinceHandler(mockWebLocalProvinceSqlRepository.Object, mockWebLocalSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateWebLocalProvinceRequest
            {
                Name = "Trà Vinh",
                NameEn = "Tra Vinh",
                FullName = "Tỉnh Trà Vinh",
                FullNameEn = "Tra Vinh Province",
                Latitude = 2.2,
                Longitude = 2.1,
                KeyLocalization = "russia"
            };

            Expression<Func<Entities.WebLocals, bool>> predicate = x => (x.Id == request.KeyLocalization);
            mockWebLocalSqlRepository
                .Setup(repo => repo.IsExistAsync(predicate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Domain.Entities.Define.WebLocalProvince>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<int>());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockWebLocalProvinceSqlRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.Define.WebLocalProvince>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateWebLocalProvinceRequest
            {
                Name = "Trà Vinh",
                NameEn = "Tra Vinh",
                FullName = "Tỉnh Trà Vinh",
                FullNameEn = "Tra Vinh Province",
                Latitude = 2.2,
                Longitude = 2.1,
                KeyLocalization = "russia"
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Domain.Entities.Define.WebLocalProvince>()))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }

        [Fact]
        public Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateWebLocalProvinceRequest
            {
                Name = "Trà Vinh Trà Vinh Trà Vinh Trà Vinh Trà Vinh Trà Vinh Trà Vinh",
                //NameEn = "Tra Vinh",
                //FullName = "Tỉnh Trà Vinh",
                FullNameEn = "Tra Vinh Province Tra Vinh Province Tra Vinh Province Tra Vinh Province",
                //Latitude = 2.2,
                Longitude = 2.1,
                KeyLocalization = "russiarussiarussiarussiarussiarussiarussiarussiarussiarussiarussiarussiarussia"
            };

            var validator = new CreateWebLocalProvinceValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PROVINCE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Handler_KeyLocalizationNotFound()
        {
            // Arrange
            var request = new CreateWebLocalProvinceRequest
            {
                Name = "Trà Vinh",
                NameEn = "Tra Vinh",
                FullName = "Tỉnh Trà Vinh",
                FullNameEn = "Tra Vinh Province",
                Latitude = 2.2,
                Longitude = 2.1,
                KeyLocalization = "russia"
            };

            mockWebLocalSqlRepository
                .Setup(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Entities.WebLocals, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    CustomException.ThrowNotFoundException(typeof(Entities.WebLocals), MsgCode.ERR_KEY_LOCAL_NOT_FOUND, WebLocalConst.MSG_KEY_LOCALIZATION_NOT_FOUND);
                    return false; 
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));

            mockWebLocalProvinceSqlRepository.Verify(repo => repo.Add(It.IsAny<Domain.Entities.Define.WebLocalProvince>()), Times.Never);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
