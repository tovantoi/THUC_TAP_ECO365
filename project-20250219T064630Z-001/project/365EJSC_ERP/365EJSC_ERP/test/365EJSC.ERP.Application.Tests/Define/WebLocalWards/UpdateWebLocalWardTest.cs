using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Moq;

namespace _365EJSC.ERP.Application.Tests.Define.WebLocalWards
{
    public class UpdateWebLocalWardTest
    {
        private readonly Mock<IWebLocalWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<IWebLocalDictrictSqlRepository> mockDictrictSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateWebLocalWardHandler handler;

        public UpdateWebLocalWardTest()
        {
            mockWardSqlRepository = new Mock<IWebLocalWardSqlRepository>();
            mockDictrictSqlRepository = new Mock<IWebLocalDictrictSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateWebLocalWardHandler(mockWardSqlRepository.Object, mockSqlUnitOfWork.Object, mockDictrictSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_WardNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateWebLocalWardRequest { Id = 1 };

            mockWardSqlRepository
                .Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_WARD_ID_NOT_FOUND
                });

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_WARD_ID_NOT_FOUND, e.MessageCode);
            }
        }
        [Fact]
        public async Task Handle_ShouldThrowCustomException_WhenIdIsNull()
        {
            // Arrange
            var command = new UpdateWebLocalWardRequest { Id = null };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        // Test for greater than 0
        [Fact]
        public async Task Handle_ShouldThrowCustomException_WhenWardIdIsLessThanOrEqualToZero()
        {
            // Arrange
            var command = new UpdateWebLocalWardRequest { Id = 1, DistrictId = 0 };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Handle_ShouldThrowCustomException_WhenRequestIsInvalid()
        {
            // Arrange
            var request = new UpdateWebLocalWardRequest { Id = 0, DistrictId = 0 }; // Invalid ID & DistrictId

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
        }

    }
}
