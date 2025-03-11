using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Application.UserCases.Define.GeneralDepartments;
using _365EJSC.ERP.Application.Validators.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.GeneralDepartments
{
    /// <summary>
    /// Test class for GetGeneralDepartmentByIdQueryHandler.
    /// </summary>
    public class GetDetailGeneralDepartmentTest
    {
        private readonly Mock<IGeneralDepartmentSqlRepository> mockGeneralDepartmentRepository;
        private readonly GetDetailGeneralDepartmentHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGeneralDepartmentByIdTest"/> class.
        /// </summary>
        public GetDetailGeneralDepartmentTest()
        {
            mockGeneralDepartmentRepository = new Mock<IGeneralDepartmentSqlRepository>();
            handler = new GetDetailGeneralDepartmentHandler(mockGeneralDepartmentRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnGeneralDepartment_When_Found()
        {
            // Arrange
            var generalDepartment = new GeneralDepartment { Id = 1, DeName = "HR" };
            mockGeneralDepartmentRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(generalDepartment);
            var query = new GetDetailGeneralDepartmentRequest { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(generalDepartment, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_GeneralDepartment_NotFound()
        {
            // Arrange
            mockGeneralDepartmentRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_DEPARTMENT_ID_NOT_FOUND
                });
            var query = new GetDetailGeneralDepartmentRequest { Id = 99 };

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
                Assert.Equal(MsgCode.ERR_DEPARTMENT_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailGeneralDepartmentRequest
            {
                // Set invalid properties of GetDetailGeneralDepartmentQuery here
            };

            var validator = new GetDetailGeneralDepartmentValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_DEPARTMENT_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
