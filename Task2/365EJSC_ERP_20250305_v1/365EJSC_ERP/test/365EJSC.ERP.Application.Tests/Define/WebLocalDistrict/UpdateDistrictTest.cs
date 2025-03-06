using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalDistrict
{
    public class UpdateDistrictTest
    {
        private readonly Mock<IWebLocalDistrictSqlRepository> mockDistrictRepository;
        private readonly Mock<IWebLocalProvinceSqlRepository> mockProvinceSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateWebLocalDistrictHandler handler;

        public UpdateDistrictTest()
        {
            mockDistrictRepository = new Mock<IWebLocalDistrictSqlRepository>();
            mockProvinceSqlRepository = new Mock<IWebLocalProvinceSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateWebLocalDistrictHandler(mockDistrictRepository.Object, mockProvinceSqlRepository.Object, mockSqlUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new UpdateWebLocalDistrictRequest
            {
                Id = 1,
                Name = "Updated District",
                FullName = "Updated Full Name of District",
                Latitude = 21.1234,
                Longitude = 105.6789,
                ProvinceId = 1
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(mockTransaction.Object);

            mockDistrictRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new Entities.WebLocalDistrict { Id = 1 }); // Simulate finding the district

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));  // Expecting CustomException
        }

    }
}

