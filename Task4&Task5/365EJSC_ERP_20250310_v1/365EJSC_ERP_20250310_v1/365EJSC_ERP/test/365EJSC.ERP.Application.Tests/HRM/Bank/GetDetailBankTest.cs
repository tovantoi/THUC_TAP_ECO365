using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Application.UserCases.HRM.Bank;
using _365EJSC.ERP.Application.Validators.HRM.Bank;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using Moq;

namespace _365EJSC.ERP.Application.Tests.HRM.Bank
{
    public class GetDetailBankTest
    {
        private readonly Mock<IBankSqlRepository> mockBankSqlRepository;
        private readonly GetDetailBankHandler handler;

        public GetDetailBankTest()
        {
            mockBankSqlRepository = new Mock<IBankSqlRepository>();
            handler = new GetDetailBankHandler(mockBankSqlRepository.Object);
        }
        [Fact]
        public async Task Handle_Should_ReturnBank_When_Found()
        {
            // Arrange
            var sample = new HrmBank { Id = 1, Name = "Sample 1" };
            mockBankSqlRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sample);
            var query = new GetDetailBankRequest { Id = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(sample, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_Bank_NotFound()
        {
            // Arrange
            mockBankSqlRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_BANK_ID_NOT_FOUND
                });
            var query = new GetDetailBankRequest { Id = 99 };

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
                Assert.Equal(MsgCode.ERR_BANK_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailBankRequest
            {
                // Set invalid properties of CreateSampleCommand here
            };

            var validator = new GetDetailBankValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_BANK_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}
