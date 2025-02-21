using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalWard;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace _365EJSC.ERP.Application.Tests.Define
{
    public class UpdateWardTest
    {
        private readonly Mock<IWardSqlRepository> mockWardSqlRepository;
        private readonly Mock<IDictrictSqlRepository> mockDictrictSqlRepository;
        private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
        private readonly UpdateWardHandler handler;

        public UpdateWardTest()
        {
            mockWardSqlRepository = new Mock<IWardSqlRepository>();
            mockDictrictSqlRepository = new Mock<IDictrictSqlRepository>();
            mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
            handler = new UpdateWardHandler(mockWardSqlRepository.Object, mockSqlUnitOfWork.Object, mockDictrictSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_WardNotFound_ThrowsCustomException()
        {
            // Arrange
            var request = new UpdateWardCommand { Id = 1 };

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
            var command = new UpdateWardCommand { Id = null };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }

        // Test for greater than 0
        [Fact]
        public async Task Handle_ShouldThrowCustomException_WhenWardIdIsLessThanOrEqualToZero()
        {
            // Arrange
            var command = new UpdateWardCommand { Id = 1, DistrictId = 0 };

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Handle_ShouldThrowCustomException_WhenRequestIsInvalid()
        {
            // Arrange
            var request = new UpdateWardCommand { Id = 0, DistrictId = 0 }; // Invalid ID & DistrictId

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
        }

    }
}
