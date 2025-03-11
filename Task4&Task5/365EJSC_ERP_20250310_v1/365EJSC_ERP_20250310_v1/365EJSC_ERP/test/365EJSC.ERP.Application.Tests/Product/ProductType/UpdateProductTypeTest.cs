using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.UserCases.Product.ProductType;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.Product.ProductType
{
    public class UpdateProductTypeTest
    {
        private readonly Mock<IProductTypeSqlRepository> mockProductTypeSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateProductTypeHandler handler;

        public UpdateProductTypeTest()
        {
            mockProductTypeSqlRepository = new Mock<IProductTypeSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateProductTypeHandler(mockProductTypeSqlRepository.Object, mockSqlUnitOfWork.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new UpdateProductTypeRequest
            {
                Id = 1,
                Name = "Updated Name"
            };
            var sample = new PdProductType();
            var mockTransaction = new Mock<IDbTransaction>();

            mockProductTypeSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockProductTypeSqlRepository.Verify(repo => repo.Update(sample), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_SampleNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateProductTypeRequest
            {
                Id = 1,
                Name = "Updated Name"
            };
            mockProductTypeSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_PRODUCT_TYPE_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PRODUCT_TYPE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public async Task Handle_ExceptionOccurs_TransactionRollsBack()
        {
            // Arrange
            var request = new UpdateProductTypeRequest
            {
                Id = 1,
                Name = "Updated Name"
            };
            var sample = new PdProductType();
            var mockTransaction = new Mock<IDbTransaction>();

            mockProductTypeSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockProductTypeSqlRepository
                .Setup(repo => repo.Update(sample))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
        }
    }
}
