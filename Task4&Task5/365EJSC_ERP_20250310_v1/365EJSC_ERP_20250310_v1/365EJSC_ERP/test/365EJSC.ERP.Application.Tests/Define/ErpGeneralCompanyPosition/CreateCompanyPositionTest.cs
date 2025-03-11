using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompanyPosition
{
    public class CreateCompanyPositionTest
    {
        private readonly Mock<ICompanyPositionSqlRepository> mockCompanyPositionSqlRepository;
        private readonly Mock<ICompanySqlRepository> mockCompanySqlRepository;
        private readonly Mock<IErpGeneralPositionSqlRepository> mockPositionSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateCompanyPositionHandler handler;

        public CreateCompanyPositionTest()
        {
            mockCompanyPositionSqlRepository = new Mock<ICompanyPositionSqlRepository>();
            mockCompanySqlRepository = new Mock<ICompanySqlRepository>();
            mockPositionSqlRepository = new Mock<IErpGeneralPositionSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();

            // Pass đầy đủ các dependency vào handler
            handler = new CreateCompanyPositionHandler(
                mockCompanyPositionSqlRepository.Object,
                mockCompanySqlRepository.Object,
                mockSqlUnitOfWork.Object,
                mockPositionSqlRepository.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateCompanyPositionRequest
            {
                CompanyId = 1,
                PositionIds = new List<int> { 1, 2 }
            };

            var company = new Entities.ErpGeneralCompany { Id = 1, IsActived = true };
            var departments = new List<Entities.ErpGeneralPosition>
            {
                new Entities.ErpGeneralPosition { Id = 1 },
                new Entities.ErpGeneralPosition { Id = 2 }
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync(request.CompanyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockPositionSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(departments);

            mockCompanyPositionSqlRepository
                .Setup(repo => repo.FindAll(x => x.CompanyId == request.CompanyId, false))
                .Returns(new List<Entities.ErpGeneralCompanyPosition>().AsQueryable());

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
            mockCompanyPositionSqlRepository.Verify(repo => repo.AddRange(It.IsAny<List<Entities.ErpGeneralCompanyPosition>>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_ExistingPositions_ThrowsConflictException()
        {
            // Arrange
            var request = new CreateCompanyPositionRequest
            {
                CompanyId = 1,
                PositionIds = new List<int> { 1, 2 }
            };

            var company = new Entities.ErpGeneralCompany { Id = 1, IsActived = true };
            var departments = new List<Entities.ErpGeneralPosition>
            {
                new Entities.ErpGeneralPosition { Id = 1 },
                new Entities.ErpGeneralPosition { Id = 2 }
            };

            var existingCompanyPositions = new List<Entities.ErpGeneralCompanyPosition>
            {
                new Entities.ErpGeneralCompanyPosition { CompanyId = 1, PositionId = 1 }
            }.AsQueryable();

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync(request.CompanyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockPositionSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(departments);

            mockCompanyPositionSqlRepository
                .Setup(repo => repo.FindAll(x => x.CompanyId == request.CompanyId, false))
                .Returns(existingCompanyPositions);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_POSITION_DUPLICATE_ID, exception.MessageCode);
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateCompanyPositionRequest
            {
                CompanyId = 1,
                PositionIds = new List<int> { 1 }
            };

            var company = new Entities.ErpGeneralCompany { Id = 1, IsActived = true };
            var departments = new List<Entities.ErpGeneralPosition>
            {
                new Entities.ErpGeneralPosition { Id = 1 }
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync(request.CompanyId, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockPositionSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(departments);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanyPositionSqlRepository
                .Setup(repo => repo.AddRange(It.IsAny<List<Entities.ErpGeneralCompanyPosition>>()))
                .Throws(new Exception("Create failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}