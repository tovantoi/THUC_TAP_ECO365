using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.UserCases.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Validators.HRM.EmployeeRole;
using _365EJSC.ERP.Application.Validators.HRM.Marital;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.EmployeeRole
{
    public class CreateEmployeeRoleTest
    {
        private readonly Mock<IEmployeeRoleSqlRepository> mockemployeeRoleSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly CreateEmployeeRoleHandler handler;

        public CreateEmployeeRoleTest()
        {
            mockemployeeRoleSqlRepository = new Mock<IEmployeeRoleSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new CreateEmployeeRoleHandler(mockemployeeRoleSqlRepository.Object, mockSqlUnitOfWork.Object);
        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = new CreateEmployeeRoleRequest
            {
                Code = "Nv01",
                Name = "test"
            };

            var mockTransaction = new Mock<IDbTransaction>();

            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockemployeeRoleSqlRepository
                .Setup(repo => repo.Add(It.IsAny<HrmEmployeeRole>()));

            mockSqlUnitOfWork
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockemployeeRoleSqlRepository.Verify(repo => repo.Add(It.IsAny<HrmEmployeeRole>()), Times.Once);
            mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
            mockTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
        {
            // Arrange
            var request = new CreateEmployeeRoleRequest
            {
                Code = "Nv01",
                Name = "test"
            };

            var mockTransaction = new Mock<IDbTransaction>();
            mockSqlUnitOfWork
                .Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            mockemployeeRoleSqlRepository
                .Setup(repo => repo.Add(It.IsAny<HrmEmployeeRole>()))
                .Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            mockTransaction.Verify(t => t.Rollback(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Never);
        }

        [Fact]
        public Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new CreateEmployeeRoleRequest
            {
                // Set invalid properties of CreateMaritalRequest here
            };

            var validator = new CreateEmployeeRoleValidator();
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
