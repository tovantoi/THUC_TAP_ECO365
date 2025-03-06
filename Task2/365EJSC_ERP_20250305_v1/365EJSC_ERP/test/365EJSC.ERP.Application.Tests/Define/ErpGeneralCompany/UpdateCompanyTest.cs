using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompany
{
    public class UpdateCompanyTest
    {
        private readonly Mock<ICompanySqlRepository> mockCompanyRepository;
        private readonly Mock<IWebLocalWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<IWebLocalSqlRepository> mockLocalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateCompanyHandler handler;

        public UpdateCompanyTest()
        {
            mockCompanyRepository = new Mock<ICompanySqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            mockWardSqlRepository = new Mock<IWebLocalWardSqlRepository>();
            mockLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            handler = new UpdateCompanyHandler(mockCompanyRepository.Object, mockSqlUnitOfWork.Object, mockWardSqlRepository.Object, mockLocalSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new UpdateCompanyRequest
            {
                Id = 1,
                WardId = 1
            };

            var company = new Entities.ErpGeneralCompany();
            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanyRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockLocalSqlRepository
                .Setup(static repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocals { Id = "AAA", Localization = "Test", IsActived = true });

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalWard { Id = 1, Name = "Test Ward" });

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockCompanyRepository.Verify(repo => repo.Update(company), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_CompanyNotFound_ThrowsCustomException()
        {
            var request = new UpdateCompanyRequest { Id = 1 };

            mockCompanyRepository.Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Entities.ErpGeneralCompany)null);

            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_COMPANY_ID_NOT_FOUND, exception.MessageCode);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new UpdateCompanyRequest { Id = 1, WardId = 1 };
            var company = new Entities.ErpGeneralCompany();
            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanyRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockLocalSqlRepository
                .Setup(static repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocals { Id = "AAA", Localization = "Test", IsActived = true });

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalWard { Id = 1, Name = "Test Ward" });

            mockCompanyRepository
                .Setup(repo => repo.Update(company))
                .Throws(new Exception("Update failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}