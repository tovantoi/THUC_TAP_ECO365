using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompanyDepartment
{
    public class UpdateCompanyDepartmentTest
    {
        private readonly Mock<ICompanyDepartmentSqlRepository> mockCompanyDepartmentSqlRepository;
        private readonly Mock<ICompanySqlRepository> mockCompanySqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateCompanyDepartmentHandler handler;

        public UpdateCompanyDepartmentTest()
        {
            mockCompanyDepartmentSqlRepository = new Mock<ICompanyDepartmentSqlRepository>();
            mockCompanySqlRepository = new Mock<ICompanySqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateCompanyDepartmentHandler(mockCompanyDepartmentSqlRepository.Object, mockCompanySqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new UpdateCompanyDepartmentRequest
            {
                Id = 1,
                CompanyId = 1,
                DepartmentId = 1
            };

            var companyDepartment = new Entities.ErpGeneralCompanyDepartment();
            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanyDepartmentSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDepartment);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.ErpGeneralCompany { Id = 1, IsActived = true });

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockCompanyDepartmentSqlRepository.Verify(repo => repo.Update(companyDepartment), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_CompanyDepartmentNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateCompanyDepartmentRequest { Id = 1, CompanyId = 99 };

            mockCompanyDepartmentSqlRepository.Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                                    .ReturnsAsync((Entities.ErpGeneralCompanyDepartment)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_COMPANY_DEPARTMENT_ID_NOT_FOUND, exception.MessageCode);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new UpdateCompanyDepartmentRequest { Id = 1, CompanyId = 1 };
            var companyDepartment = new Entities.ErpGeneralCompanyDepartment();
            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanyDepartmentSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDepartment);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.ErpGeneralCompany { Id = 1, IsActived = true });

            mockCompanyDepartmentSqlRepository
                .Setup(repo => repo.Update(companyDepartment))
                .Throws(new Exception("Update failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}