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
    public class CreateCompanyDepartmentTest
    {
        private readonly Mock<ICompanyDepartmentSqlRepository> mockCompanyDepartmentSqlRepository;
        private readonly Mock<ICompanySqlRepository> mockCompanySqlRepository;
        private readonly Mock<IGeneralDepartmentSqlRepository> mockDepartmentSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateCompanyDepartmentHandler handler;

        public CreateCompanyDepartmentTest()
        {
            mockCompanyDepartmentSqlRepository = new Mock<ICompanyDepartmentSqlRepository>();
            mockCompanySqlRepository = new Mock<ICompanySqlRepository>();
            mockDepartmentSqlRepository = new Mock<IGeneralDepartmentSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();

            // Pass đầy đủ các dependency vào handler
            handler = new CreateCompanyDepartmentHandler(
                mockCompanyDepartmentSqlRepository.Object,
                mockCompanySqlRepository.Object,
                mockSqlUnitOfWork.Object,
                mockDepartmentSqlRepository.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateCompanyDepartmentRequest
            {
                CompanyId = 1,
                DepartmentIds = new List<int> { 1, 2 }
            };

            var company = new Entities.ErpGeneralCompany { Id = 1, IsActived = true };
            var departments = new List<Entities.GeneralDepartment>
            {
                new Entities.GeneralDepartment { Id = 1 },
                new Entities.GeneralDepartment { Id = 2 }
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync(request.CompanyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockDepartmentSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(departments);

            mockCompanyDepartmentSqlRepository
                .Setup(repo => repo.FindAll(x => x.CompanyId == request.CompanyId, false))
                .Returns((new List<Entities.ErpGeneralCompanyDepartment>()).AsQueryable());

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockCompanyDepartmentSqlRepository.Verify(repo => repo.AddRange(It.IsAny<List<Entities.ErpGeneralCompanyDepartment>>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_ExistingDepartments_ThrowsConflictException()
        {
            // Arrange
            var request = new CreateCompanyDepartmentRequest
            {
                CompanyId = 1,
                DepartmentIds = new List<int> { 1, 2 }
            };

            var company = new Entities.ErpGeneralCompany { Id = 1, IsActived = true };
            var departments = new List<Entities.GeneralDepartment>
            {
                new Entities.GeneralDepartment { Id = 1 },
                new Entities.GeneralDepartment { Id = 2 }
            };

            var existingCompanyDepartments = new List<Entities.ErpGeneralCompanyDepartment>
            {
                new Entities.ErpGeneralCompanyDepartment { CompanyId = 1, DepartmentId = 1 }
            }.AsQueryable();

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync(request.CompanyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockDepartmentSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(departments);

            mockCompanyDepartmentSqlRepository
                .Setup(repo => repo.FindAll(x => x.CompanyId == request.CompanyId, false))
                .Returns(existingCompanyDepartments);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_DEPARTMENT_DUPLICATE_ID, exception.MessageCode);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateCompanyDepartmentRequest
            {
                CompanyId = 1,
                DepartmentIds = new List<int> { 1 }
            };

            var company = new Entities.ErpGeneralCompany { Id = 1, IsActived = true };
            var departments = new List<Entities.GeneralDepartment>
            {
                new Entities.GeneralDepartment { Id = 1 }
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync(request.CompanyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockDepartmentSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(departments);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanyDepartmentSqlRepository
                .Setup(repo => repo.AddRange(It.IsAny<List<Entities.ErpGeneralCompanyDepartment>>()))
                .Throws(new Exception("Create failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}