using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Application.UserCases.HRM.Degree;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Validators.HRM.Degree;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.Tests.HRM.Degree
{
	public class GetDegreeByIdTest
	{
		private readonly Mock<IDegreeSqlRepository> mockDegreeSqlRepository;
		private readonly GetDetailDegreeHandler handler;

		public GetDegreeByIdTest()
		{
			mockDegreeSqlRepository = new Mock<IDegreeSqlRepository>();
			handler = new GetDetailDegreeHandler(mockDegreeSqlRepository.Object);
		}
		[Fact]
		public async Task Handle_Should_ReturnDegree_When_Found()
		{
			// Arrange
			var degree = new Entities.Degree
			{
				Id = 1,
				Name = "Bachelor of Science"
			};

			var query = new GetDetailDegreeRequest { Id = 1 };
			mockDegreeSqlRepository.Setup(r => r.FindByIdAsync(query.Id, false, It.IsAny<CancellationToken>()))
				.ReturnsAsync(degree);

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(degree, result.Data);
		}
		[Fact]
		public async Task Handle_Should_ThrowException_When_Degree_NotFound()
		{
			var request = new GetDetailDegreeRequest { Id = 99 };

			// Arrange
			mockDegreeSqlRepository
				.Setup(repo => repo.FindByIdAsync(request.Id, false, It.IsAny<CancellationToken>()))
				.ReturnsAsync((Entities.Degree)null);

			// Act & Assert
			await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_Should_ThrowException_When_Request_Invalid()
		{
			// Arrange
			var request = new GetDetailDegreeRequest { Id = 0 };
			var validator = new GetDetailDegreeValidator();

			// Act & Assert
			await Assert.ThrowsAsync<CustomException>(async () => validator.ValidateAndThrow(request));
		}
	}
}
