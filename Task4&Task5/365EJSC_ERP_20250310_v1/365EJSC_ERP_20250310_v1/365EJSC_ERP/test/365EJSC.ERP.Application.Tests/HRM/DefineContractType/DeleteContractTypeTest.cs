using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Application.UserCases.HRM.DefineContractTypes;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.DefineContractType
{
    public class DeleteContractTypeTest
    {
        private readonly Mock<IContractTypeSqlRepository> mockContractTypeSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly DeleteContractTypeHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteContractTypeTest"/> class.
        /// </summary>
        public DeleteContractTypeTest()
        {
            mockContractTypeSqlRepository = new Mock<IContractTypeSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new DeleteContractTypeHandler(mockContractTypeSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new DeleteContractTypeRequest { Id = 1 };
            var defineContractType = new Domain.Entities.HRM.DefineContractType
            {
                Id = 1,
                Code = "Test",
                Name = "Test"
            };
            var mockTransaction = new Mock<IDbTransaction>();

            mockContractTypeSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(defineContractType);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockContractTypeSqlRepository.Verify(repo => repo.Remove(defineContractType), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }


        [Fact]
        public async Task Handle_ContractTypeNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new DeleteContractTypeRequest { Id = 1 };

            mockContractTypeSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_CONTRACT_TYPE_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_CONTRACT_TYPE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new DeleteContractTypeRequest { Id = 1 };

            var defineContractType = new Domain.Entities.HRM.DefineContractType();
            var mockTransaction = new Mock<IDbTransaction>();

            mockContractTypeSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(defineContractType);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockContractTypeSqlRepository
                .Setup(repo => repo.Remove(defineContractType));

            // Act & Assert
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}
