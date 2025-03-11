using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.SalaryStructure
{
    public class GetDetailSalaryStructureTest
    {
        private readonly Mock<ISalaryStructureSqlRepository> mockSalaryStructureSqlRepository;
        private readonly GetDetailSalaryStructureHandler handler;

        public GetDetailSalaryStructureTest()
        {
            mockSalaryStructureSqlRepository = new Mock<ISalaryStructureSqlRepository>();
            handler = new GetDetailSalaryStructureHandler(mockSalaryStructureSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_SalaryStructure_NotFound()
        {
            var request = new GetDetailSalaryStructureRequest { Id = 99 };

            // Arrange
            mockSalaryStructureSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.HRM.DefineSalaryStructure)null);

            // Act & Assert

        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailSalaryStructureRequest
            {
                // 
            };

            var validator = new GetDetailSalaryStructureValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_SALARY_STRUCTURE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}