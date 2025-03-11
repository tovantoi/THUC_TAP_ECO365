using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Test class for creating Company entities.
    /// </summary>
    public class CreateCompanyTest
    {
        private readonly Mock<ICompanySqlRepository> mockCompanySqlRepository;
        private readonly Mock<IWebLocalWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<IWebLocalSqlRepository> mockLocalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateCompanyHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCompanyTest"/> class.
        /// </summary>
        public CreateCompanyTest()
        {
            mockCompanySqlRepository = new Mock<ICompanySqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            mockWardSqlRepository = new Mock<IWebLocalWardSqlRepository>();
            mockLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            handler = new CreateCompanyHandler(mockCompanySqlRepository.Object, mockSqlUnitOfWork.Object, mockWardSqlRepository.Object, mockLocalSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateCompanyRequest
            {
                CompanyPid = 1,
                TaxCode = "AAA",
                Name = "test",
                Image = "test",
                Tel = "test",
                Email = "test",
                Website = "test",
                Founder = "test",
                Ceo = "test",
                CeoImage = "test",
                CeoTel = "test",
                CeoEmail = "test",
                License = "test",
                CountryId = "AAA",
                WardId = 1,
                IsActived = true,
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanySqlRepository
                .Setup(repo => repo.Add(It.IsAny<Entities.ErpGeneralCompany>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<int>());

            mockLocalSqlRepository
                .Setup(static repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocals { Id = "AAA", Localization = "Test", IsActived = true });

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalWard { Id = 1, Name = "Test Ward" });

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockCompanySqlRepository.Verify(repo => repo.Add(It.IsAny<Entities.ErpGeneralCompany>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateCompanyRequest
            {
                CompanyPid = 1,
                TaxCode = "AAA",
                Name = "test",
                Image = "test",
                Tel = "test",
                Email = "test",
                Website = "test",
                Founder = "test",
                Ceo = "test",
                CeoImage = "test",
                CeoTel = "test",
                CeoEmail = "test",
                License = "test",
                CountryId = "AAA",
                WardId = 1,
                IsActived = true,
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanySqlRepository
                .Setup(repo => repo.Add(It.IsAny<Entities.ErpGeneralCompany>()))
                .Throws(new Exception());

            mockLocalSqlRepository
                .Setup(static repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocals { Id = "AAA", Localization = "Test", IsActived = true });

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalWard { Id = 1, Name = "Test Ward" });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }

        [Fact]
        public Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateCompanyRequest
            {
                // Set invalid properties of CreateCompanyCommand here
            };

            var validator = new CreateCompanyValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_COMPANY_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}