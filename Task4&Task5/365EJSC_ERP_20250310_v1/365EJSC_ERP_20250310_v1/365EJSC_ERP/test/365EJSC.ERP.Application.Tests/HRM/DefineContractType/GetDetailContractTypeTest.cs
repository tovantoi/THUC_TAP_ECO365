using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Application.UserCases.HRM.DefineContractTypes;
using _365EJSC.ERP.Application.Validators.HRM.DefineContractTypes;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.DefineContractType
{
    public class GetDetailContractTypeTest
    {
        private readonly Mock<IContractTypeSqlRepository> mockContractTypeSqlRepository;
        private readonly GetDetailContractTypeHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDetailContractTypeTest"/> class.
        /// </summary>
        public GetDetailContractTypeTest()
        {
            mockContractTypeSqlRepository = new Mock<IContractTypeSqlRepository>();
            handler = new GetDetailContractTypeHandler(mockContractTypeSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_ContractType_NotFound()
        {
            // Arrange
            mockContractTypeSqlRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_CONTRACT_TYPE_ID_NOT_FOUND
                });
            var query = new GetDetailContractTypeRequest { Id = 99 };

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
                Assert.Equal(MsgCode.ERR_CONTRACT_TYPE_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailContractTypeRequest
            {
                //
            };

            var validator = new GetDetailContractTypeValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_CONTRACT_TYPE_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
