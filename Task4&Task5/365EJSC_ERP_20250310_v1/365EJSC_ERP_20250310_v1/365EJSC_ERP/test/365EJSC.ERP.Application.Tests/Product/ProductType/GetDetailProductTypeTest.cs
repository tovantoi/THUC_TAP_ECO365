using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.UserCases.Product.ProductType;
using _365EJSC.ERP.Application.Validators.Product.ProductType;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Product.ProductType
{
    public class GetDetailProductTypeTest
    {
        private readonly Mock<IProductTypeSqlRepository> mockProductTypeSqlRepository;
        private readonly GetDetailProductTypeHandler handler;

        public GetDetailProductTypeTest()
        {
            mockProductTypeSqlRepository = new Mock<IProductTypeSqlRepository>();
            handler = new GetDetailProductTypeHandler(mockProductTypeSqlRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnBank_When_Found()
        {
            // Arrange
            var sample = new PdProductType { Id = 1, Name = "Sample 1" };
            mockProductTypeSqlRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);
            var query = new GetDetailProductTypeRequest { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(sample, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_Bank_NotFound()
        {
            // Arrange
            mockProductTypeSqlRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_PRODUCT_TYPE_ID_NOT_FOUND
                });
            var query = new GetDetailProductTypeRequest { Id = 99 };

            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<CustomException>(act);
            try
            {
                await handler.Handle(query, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PRODUCT_TYPE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailProductTypeRequest
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailProductTypeValidator();
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
