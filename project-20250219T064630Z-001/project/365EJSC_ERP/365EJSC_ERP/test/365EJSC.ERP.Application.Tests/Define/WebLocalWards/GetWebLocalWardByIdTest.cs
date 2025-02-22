using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalWards
{
    public class GetWebLocalWardByIdTest
    {
        private readonly Mock<IWebLocalWardSqlRepository> mockWardRepository;


        private readonly GetDetailWebLocalWardHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSampleTest"/> class.
        /// </summary>
        public GetWebLocalWardByIdTest()
        {
            mockWardRepository = new Mock<IWebLocalWardSqlRepository>();
            handler = new GetDetailWebLocalWardHandler(mockWardRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnSample_When_Found()
        {
            // Arrange
            var sample = new WebLocalWard { Id = 1, Name = "Ward 1", WebLocalDistrict = new WebLocalDistrict { WebLocalProvince = new WebLocalProvince() } };

            mockWardRepository
                .Setup(r => r.FindByIdAsync(1, true, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<System.Func<WebLocalWard, object>>[]>()))
                .ReturnsAsync(sample);

            var query = new GetDetailWebLocalWardRequest { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Ward 1", result.Data.Name);
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Sample_NotFound()
        {
            // Arrange: Mock repository trả về null khi không tìm thấy
            mockWardRepository
                .Setup(r => r.FindByIdAsync(99, true, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<System.Func<WebLocalWard, object>>[]>()))
                .ReturnsAsync((WebLocalWard)null);

            var query = new GetDetailWebLocalWardRequest { Id = 99 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal(MsgCode.ERR_WARD_ID_NOT_FOUND, result.MessageCode);
        }

    [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailWebLocalWardRequest
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailWebLocalWardValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_WARD_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
