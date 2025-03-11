using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.SalaryStructure
{
    public class DeleteSalaryStructureTest
    {
        private readonly Mock<ISalaryStructureSqlRepository> mockSalaryStructureSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteSalaryStructureHandler handler;

        public DeleteSalaryStructureTest()
        {
            mockSalaryStructureSqlRepository = new Mock<ISalaryStructureSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteSalaryStructureHandler(mockSalaryStructureSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteSalaryStructureRequest { Id = 1 };
            var erpGeneralPosition = new Domain.Entities.HRM.DefineSalaryStructure();
            var mockTransaction = new Mock<IDbTransaction>();

            mockSalaryStructureSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(erpGeneralPosition);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockSalaryStructureSqlRepository.Verify(repo => repo.Remove(erpGeneralPosition), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_WebLocalProvinceNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteSalaryStructureRequest { Id = 1 };

            mockSalaryStructureSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_SALARY_STRUCTURE_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_SALARY_STRUCTURE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteSalaryStructureRequest { Id = 1 };
            var erpGeneralPosition = new Domain.Entities.HRM.DefineSalaryStructure();
            var mockTransaction = new Mock<IDbTransaction>();

            mockSalaryStructureSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(erpGeneralPosition);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockSalaryStructureSqlRepository
                .Setup(repo => repo.Remove(erpGeneralPosition))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }
    }
}