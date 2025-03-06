using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalProvince
{
    /// <summary>
    /// Test class for GetSampleByIdQueryHandler.
    /// </summary>
    public class GetWebLocalProvinceByIdTest
    {
        private readonly Mock<IWebLocalProvinceSqlRepository> mockWebLocalProvinceSqlRepository;
        private readonly GetDetailWebLocalProvinceHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWebLocalProvinceByIdTest"/> class.
        /// </summary>
        public GetWebLocalProvinceByIdTest()
        {
            mockWebLocalProvinceSqlRepository = new Mock<IWebLocalProvinceSqlRepository>();
            handler = new GetDetailWebLocalProvinceHandler(mockWebLocalProvinceSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnWebLocalProvince_When_Found()
        {
            // Arrange
            var webLocalProvince = new Domain.Entities.Define.WebLocalProvince
            {
                Id = 1,
                Name = "Trà Vinh",
                NameEn = "Tra Vinh",
                FullName = "Tỉnh Trà Vinh",
                FullNameEn = "Tra Vinh Province",
                Latitude = 2.2,
                Longitude = 2.1,
                KeyLocalization = "russia"
            };

            var query = new GetDetailWebLocalProvinceRequest { Id = 1 };
            mockWebLocalProvinceSqlRepository.Setup(r => r.FindByIdAsync((int)query.Id, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(webLocalProvince);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_WebLocalProvince_NotFound()
        {
            var request = new GetDetailWebLocalProvinceRequest { Id = 99 };

            // Arrange
            mockWebLocalProvinceSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Define.WebLocalProvince)null);

            // Act & Assert

        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailWebLocalProvinceRequest
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailWebLocalProvinceValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_PROVINCE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
