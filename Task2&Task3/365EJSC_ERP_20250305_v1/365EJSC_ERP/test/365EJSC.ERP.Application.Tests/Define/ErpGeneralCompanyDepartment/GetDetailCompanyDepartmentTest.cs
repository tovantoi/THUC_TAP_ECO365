using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompanyDepartment
{
    public class GetDetailCompanyDepartmentTest
    {
        private readonly Mock<ICompanyDepartmentSqlRepository> mockCompanyDepartmentRepository;
        private readonly GetDetailCompanyDepartmentHandler handler;

        public GetDetailCompanyDepartmentTest()
        {
            mockCompanyDepartmentRepository = new Mock<ICompanyDepartmentSqlRepository>();
            handler = new GetDetailCompanyDepartmentHandler(mockCompanyDepartmentRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnCompanyDepartment_When_Found()
        {
            // Arrange
            var companyDepartment = new Entities.ErpGeneralCompanyDepartment
            {
                Id = 1,
                CompanyId = 1,
                DepartmentId = 1
            };

            mockCompanyDepartmentRepository
                .Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDepartment);

            var request = new GetDetailCompanyDepartmentRequest { Id = 1 };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyDepartment, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_CompanyDepartment_NotFound()
        {
            // Arrange
            mockCompanyDepartmentRepository
                .Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Entities.ErpGeneralCompanyDepartment)null);

            var request = new GetDetailCompanyDepartmentRequest { Id = 99 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal(MsgCode.ERR_COMPANY_DEPARTMENT_ID_NOT_FOUND, exception.MessageCode);
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailCompanyDepartmentRequest();

            var validator = new GetDetailCompanyDepartmentValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_COMPANY_DEPARTMENT_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}