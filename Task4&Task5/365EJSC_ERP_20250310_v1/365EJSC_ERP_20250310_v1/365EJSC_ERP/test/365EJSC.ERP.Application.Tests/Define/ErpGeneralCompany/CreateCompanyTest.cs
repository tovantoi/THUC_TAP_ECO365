using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompany
{
    public class CreateCompanyTest
    {
        private readonly Mock<ICompanySqlRepository> mockCompanySqlRepository;
        private readonly Mock<IWebLocalWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<IWebLocalSqlRepository> mockLocalSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly Mock<IGeneralDepartmentSqlRepository> mockDepartmentSqlRepository;
        private readonly Mock<IErpGeneralPositionSqlRepository> mockPositionSqlRepository;
        private readonly CreateCompanyHandler handler;

        public CreateCompanyTest()
        {
            mockCompanySqlRepository = new Mock<ICompanySqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            mockWardSqlRepository = new Mock<IWebLocalWardSqlRepository>();
            mockLocalSqlRepository = new Mock<IWebLocalSqlRepository>();
            mockDepartmentSqlRepository = new Mock<IGeneralDepartmentSqlRepository>();
            mockPositionSqlRepository = new Mock<IErpGeneralPositionSqlRepository>();

            handler = new CreateCompanyHandler(
                mockCompanySqlRepository.Object,
                mockSqlUnitOfWork.Object,
                mockWardSqlRepository.Object,
                mockLocalSqlRepository.Object,
                mockDepartmentSqlRepository.Object,
                new Mock<ICompanyDepartmentSqlRepository>().Object,
                mockPositionSqlRepository.Object,
                new Mock<ICompanyPositionSqlRepository>().Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateCompanyRequest
            {
                CompanyPid = 1,
                TaxCode = "AAA",
                Name = "Test Company",
                Image = "image_url",
                Tel = "123456789",
                Email = "test@example.com",
                Website = "www.example.com",
                Founder = "John Doe",
                Ceo = "Jane Smith",
                CeoImage = "ceo_image_url",
                CeoTel = "987654321",
                CeoEmail = "ceo@example.com",
                License = "XYZ123",
                CountryId = "AAA",
                WardId = 1,
                IsActived = true,
                DepartmentIds = new List<int> { 1, 2 },
                PositionIds = new List<int> { 3, 4 }
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanySqlRepository
                .Setup(repo => repo.Add(It.IsAny<Entities.ErpGeneralCompany>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mockLocalSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocals { Id = "AAA", Localization = "Test", IsActived = true });

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalWard { Id = 1, Name = "Test Ward" });

            mockDepartmentSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Entities.GeneralDepartment>
                {
                    new Entities.GeneralDepartment { Id = 1, DeName = "HR" },
                    new Entities.GeneralDepartment { Id = 2, DeName = "IT" }
                });

            mockPositionSqlRepository
                .Setup(repo => repo.FindByIds(It.IsAny<List<int>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Entities.ErpGeneralPosition>
                {
                    new Entities.ErpGeneralPosition { Id = 3, Name = "Manager" },
                    new Entities.ErpGeneralPosition { Id = 4, Name = "Developer" }
                });

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockCompanySqlRepository.Verify(repo => repo.Add(It.IsAny<Entities.ErpGeneralCompany>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateCompanyRequest
            {
                CompanyPid = 1,
                TaxCode = "AAA",
                Name = "Test Company",
                Image = "image_url",
                Tel = "123456789",
                Email = "test@example.com",
                Website = "www.example.com",
                Founder = "John Doe",
                Ceo = "Jane Smith",
                CeoImage = "ceo_image_url",
                CeoTel = "987654321",
                CeoEmail = "ceo@example.com",
                License = "XYZ123",
                CountryId = "AAA",
                WardId = 1,
                IsActived = true,
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockCompanySqlRepository
                .Setup(repo => repo.Add(It.IsAny<Entities.ErpGeneralCompany>()))
                .Throws(new Exception());

            mockLocalSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocals { Id = "AAA", Localization = "Test", IsActived = true });

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Entities.WebLocalWard { Id = 1, Name = "Test Ward" });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }
    }
}