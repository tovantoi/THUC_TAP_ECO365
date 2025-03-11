using _365EJSC.ERP.Application.UserCases.HRM.Degree;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Requests.HRM.Degree;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;


namespace _365EJSC.ERP.Application.Tests.HRM.Degree
{
	public class GetAllDegreeTest 
	{
		private readonly Mock<IDegreeSqlRepository> mockDegreeSqlRepository;
		private readonly GetAllDegreeHandler handler;

		public GetAllDegreeTest()
		{
			mockDegreeSqlRepository = new Mock<IDegreeSqlRepository>();
			handler = new GetAllDegreeHandler(mockDegreeSqlRepository.Object);
		}
		[Fact]
		public async Task Handle_Should_ReturnAllDegrees()
		{
			// Arrange
			var degrees = new List<Entities.Degree>
			{
				new() { Id = 1, Name = "Bachelor of Science" },
				new() { Id = 2, Name = "Master of Science" }
			};
			mockDegreeSqlRepository.Setup(repository => repository.FindAll(null, false)).Returns(degrees.AsQueryable());
			var query = new GetAllDegreeRequest();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(2, result.Data.Count);
		}

		[Fact]
		public async Task Handle_Should_ReturnEmptyList_When_NoDegrees()
		{
			// Arrange
			mockDegreeSqlRepository.Setup(r => r.FindAll(null, false)).Returns(new List<Entities.Degree>().AsQueryable());
			var query = new GetAllDegreeRequest();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Empty(result.Data);
		}
	}
}
