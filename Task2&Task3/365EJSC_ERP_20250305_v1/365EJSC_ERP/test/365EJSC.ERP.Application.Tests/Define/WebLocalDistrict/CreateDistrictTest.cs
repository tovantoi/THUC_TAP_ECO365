using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalDistrict
{
    /// <summary>
    /// Test class for creating District entities.
    /// </summary>
    public class CreateDistrictTest
    {
        private readonly Mock<IWebLocalDistrictSqlRepository> mockDistrictSqlRepository;
        private readonly Mock<IWebLocalProvinceSqlRepository> mockProvinceSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateWebLocalDistrictHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDistrictTest"/> class.
        /// </summary>
        public CreateDistrictTest()
        {
            mockDistrictSqlRepository = new Mock<IWebLocalDistrictSqlRepository>();
            mockProvinceSqlRepository = new Mock<IWebLocalProvinceSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateWebLocalDistrictHandler(mockDistrictSqlRepository.Object, mockProvinceSqlRepository.Object, mockSqlUnitOfWork.Object);
        }


        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateWebLocalDistrictRequest
            {
                Name = "District B",
                FullName = "Full Name of District B",
                Latitude = 21.0285,
                Longitude = 105.8542,
                ProvinceId = 1
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockDistrictSqlRepository
                .Setup(repo => repo.Add(It.IsAny<Entities.WebLocalDistrict>()))
                .Throws(new Exception());

            // Act & Assert
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }

        [Fact]
        public Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateWebLocalDistrictRequest
            {
                // Set invalid properties of CreateWebLocalDistrictRequest here (e.g., missing name or invalid latitude)
            };

            var validator = new CreateWebLocalDistrictValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_DISTRICT_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
