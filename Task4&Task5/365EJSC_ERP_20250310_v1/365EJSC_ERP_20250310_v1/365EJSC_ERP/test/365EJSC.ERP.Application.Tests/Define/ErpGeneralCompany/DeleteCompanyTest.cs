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
    /// <summary>
    /// Test class for deleting Company entities.
    /// </summary>
    public class DeleteCompanyTest
    {
        private readonly Mock<ICompanySqlRepository> mockCompanySqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteCompanyHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCompanyTest"/> class.
        /// </summary>
        public DeleteCompanyTest()
        {
            mockCompanySqlRepository = new Mock<ICompanySqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteCompanyHandler(mockCompanySqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteCompanyRequest { Id = 1 };
            var company = new Entities.ErpGeneralCompany
            {
                Id = 1,
                IsActived = true
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanySqlRepository
                .Setup(repo => repo.FindSingleAsync(x => x.Id == request.Id && x.IsActived == true, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

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
            mockCompanySqlRepository.Verify(repo => repo.Update(It.IsAny<Entities.ErpGeneralCompany>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_CompanyNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteCompanyRequest { Id = 1 };

            mockCompanySqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_COMPANY_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_COMPANY_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteCompanyRequest { Id = 1 };
            var company = new Entities.ErpGeneralCompany();
            var mockTransaction = new Mock<IDbTransaction>();

            mockCompanySqlRepository
                .Setup(repo => repo.FindSingleAsync(x => x.Id == request.Id && x.IsActived == true, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanySqlRepository
                .Setup(repo => repo.Update(It.IsAny<Entities.ErpGeneralCompany>()))
                .Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}