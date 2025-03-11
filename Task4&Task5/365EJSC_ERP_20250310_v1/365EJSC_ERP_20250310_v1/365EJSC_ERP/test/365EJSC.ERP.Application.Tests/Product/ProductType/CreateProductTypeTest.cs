using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.UserCases.Product.ProductType;
using _365EJSC.ERP.Application.Validators.Product.ProductType;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.Product.ProductType
{
    public class CreateProductTypeTest
    {
        private readonly Mock<IProductTypeSqlRepository> mockProductTypeSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateProductTypeHandler handler;

        public CreateProductTypeTest()
        {
            mockProductTypeSqlRepository = new Mock<IProductTypeSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateProductTypeHandler(mockProductTypeSqlRepository.Object, mockSqlUnitOfWork.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateProductTypeRequest
            {
                Name = "test"
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockProductTypeSqlRepository
                .Setup(repo => repo.Add(It.IsAny<PdProductType>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockProductTypeSqlRepository.Verify(repo => repo.Add(It.IsAny<PdProductType>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateProductTypeRequest
            {
                Name = "test",
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockProductTypeSqlRepository
                .Setup(repo => repo.Add(It.IsAny<PdProductType>()))
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
            var request = new CreateProductTypeRequest
            {
                // Set invalid properties of CreateProductTypeRequest here
            };

            var validator = new CreateProductTypeValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PRODUCT_TYPE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}

