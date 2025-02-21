using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWard;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWard;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.Data;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Application.Tests.Define
{
    public class CreateWardTest
    {
        private readonly Mock<IWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<IDictrictSqlRepository> mockDictrictSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateWardHandler handler;

        public CreateWardTest()
        {
            mockWardSqlRepository = new Mock<IWardSqlRepository>();
            mockDictrictSqlRepository = new Mock<IDictrictSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateWardHandler(mockWardSqlRepository.Object, mockSqlUnitOfWork.Object, mockDictrictSqlRepository.Object);
        }


        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCustomerIdNotFound()
        {
            // Arrange
            var command = new CreateWardCommand
            {
                DistrictId = 999
            };

            mockDictrictSqlRepository
                .Setup(repo => repo.IsExistAsync(It.IsAny<Expression<Func<WebsiteLocalizationDictrict, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));

            Assert.Equal(MsgCode.ERR_WARD_INVALID, exception.MessageCode);

        }
        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenWardAlreadyExists()
        {
            // Arrange
            var command = new CreateWardCommand
            {
                NameEn = "Ward One",
                FullName = "Ward 1, District A",
                FullNameEn = "Ward One, District A",
                Latitude = 10.7769,
                Longitude = 106.7009,
                DistrictId = 2
            };

            // Mock repository cho customeraccountRepository
            mockDictrictSqlRepository.Setup(repo => repo.IsExistAsync(It.IsAny<Expression<Func<WebsiteLocalizationDictrict, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true); // Giả lập CustomerAccount tồn tại

            // Mock repository cho customerinformationRepository
            mockWardSqlRepository.Setup(repo => repo.IsExistAsync(It.IsAny<Expression<Func<WebsiteLocalizationWard, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true); // Giả lập rằng CustomerInformation đã tồn tại

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            mockWardSqlRepository.Verify(repo => repo.Add(It.IsAny<WebsiteLocalizationWard>()), Times.Never); // Không được thêm mới
        }


        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenDictrictDoesNotExist()
        {
            // Arrange
            var command = new CreateWardCommand
            {
                NameEn = "Ward One",
                FullName = "Ward 1, District A",
                FullNameEn = "Ward One, District A",
                Latitude = 10.7769,
                Longitude = 106.7009,
                DistrictId = 2
            };

            // Mock repository cho customeraccountRepository
            mockDictrictSqlRepository.Setup(repo => repo.IsExistAsync(It.IsAny<Expression<Func<WebsiteLocalizationDictrict, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false); // Giả lập CustomerAccount không tồn tại

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            mockWardSqlRepository.Verify(repo => repo.Add(It.IsAny<WebsiteLocalizationWard>()), Times.Never); // Không được thêm mới
        }

        [Fact]
        public Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateWardCommand
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new CreateWardValidator();
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

        [Fact]
        public async Task Handle_InvalidDistrictId_ThrowsNotFoundException()
        {
            // Arrange
            var request = new CreateWardCommand
            {
                NameEn = "Ward Two",
                FullName = "Ward 2, District B",
                FullNameEn = "Ward Two, District B",
                Latitude = 10.7769,
                Longitude = 106.7009,
                DistrictId = 999  // Giả sử DistrictId này không tồn tại
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockDictrictSqlRepository
                .Setup(repo => repo.IsExistAsync(It.IsAny<Expression<Func<WebsiteLocalizationDictrict, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));

            mockWardSqlRepository.Verify(repo => repo.Add(It.IsAny<WebsiteLocalizationWard>()), Times.Never);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
            mockTransaction.Verify(t => t.Rollback(), Times.Never); // Rollback không được gọi vì exception ném trước khi transaction bắt đầu thực sự.
        }

    }
}
