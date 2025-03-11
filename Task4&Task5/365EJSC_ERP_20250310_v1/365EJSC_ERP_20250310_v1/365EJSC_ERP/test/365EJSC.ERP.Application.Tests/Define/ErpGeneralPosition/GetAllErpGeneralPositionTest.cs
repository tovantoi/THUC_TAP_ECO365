using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.UserCases.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralPosition
{
    public class GetAllErpGeneralPositionTest
    {
        private readonly Mock<IErpGeneralPositionSqlRepository> mockErpGeneralPositionSqlRepository;
        private readonly GetAllErpGeneralPositionHandler handler;

        public GetAllErpGeneralPositionTest()
        {
            mockErpGeneralPositionSqlRepository = new Mock<IErpGeneralPositionSqlRepository>();
            handler = new GetAllErpGeneralPositionHandler(mockErpGeneralPositionSqlRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllErpGeneralPositions()
        {
            // Arrange
            var erpGeneralPositions = new List<Domain.Entities.Define.ErpGeneralPosition>
            {
               new() { Id = 1, Code = "", Name = "Test 1"},
               new() { Id = 2, Code = "", Name = "Test 2"},
            };
            mockErpGeneralPositionSqlRepository.Setup(repository => repository.FindAll(null, false)).Returns(erpGeneralPositions.AsQueryable());
            var query = new GetAllErpGeneralPositionRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoErpGeneralPosition()
        {
            // Arrange
            mockErpGeneralPositionSqlRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Domain.Entities.Define.ErpGeneralPosition>().AsQueryable());
            var query = new GetAllErpGeneralPositionRequest();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}
