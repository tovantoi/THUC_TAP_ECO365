using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Application.UserCases.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Validators.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.EmployeeRole
{
    public class GetDetailEmployeeRoleTest
    {
        private readonly Mock<IEmployeeRoleSqlRepository> mockemployeeRoleSqlRepository;
        private readonly GetDetailEmployeeRoleHandler handler;

        public GetDetailEmployeeRoleTest()
        {
            mockemployeeRoleSqlRepository = new Mock<IEmployeeRoleSqlRepository>();
            handler = new GetDetailEmployeeRoleHandler(mockemployeeRoleSqlRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnSample_When_Found()
        {
            // Arrange
            var sample = new HrmEmployeeRole { Id = 1, Name = "Sample 1", Code = "NV01" };
            mockemployeeRoleSqlRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);
            var query = new GetDetailEmployeeRoleRequest { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(sample, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_Sample_NotFound()
        {
            // Arrange
            mockemployeeRoleSqlRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_EMPLOYEE_ID_NOT_FOUND
                });
            var query = new GetDetailEmployeeRoleRequest { Id = 99 };

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
                Assert.Equal(MsgCode.ERR_EMPLOYEE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailEmployeeRoleRequest
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailEmployeeRoleValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_EMPLOYEE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
