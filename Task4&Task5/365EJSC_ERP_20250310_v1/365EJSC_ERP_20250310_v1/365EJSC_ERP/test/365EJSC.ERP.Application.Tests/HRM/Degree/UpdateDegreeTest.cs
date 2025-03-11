using _365EJSC.ERP.Application.UserCases.HRM.Degree;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using _365EJSC.ERP.Application.Requests.HRM.Degree;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using _365EJSC.ERP.Application.Validators.HRM.Degree;
using _365EJSC.ERP.Contract.Exceptions;

namespace _365EJSC.ERP.Application.Tests.HRM.Degree
{
	public class UpdateDegreeTest
	{
		private readonly Mock<IDegreeSqlRepository> mockDegreeSqlRepository;
		private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
		private readonly UpdateDegreeHandler handler;


		public UpdateDegreeTest()
		{
			mockDegreeSqlRepository = new Mock<IDegreeSqlRepository>();
			mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
			handler = new UpdateDegreeHandler(mockDegreeSqlRepository.Object, mockSqlUnitOfWork.Object);
		}
		[Fact]
		public async Task Handle_ValidRequest_ReturnsSuccessResult()
		{
			// Arrange
			var request = new UpdateDegreeRequest { Id = 1, Name = "Updated Degree" };
			var degree = new Entities.Degree { Id = 1, Name = "Old Degree" };
			var mockTransaction = new Mock<IDbTransaction>();

			mockDegreeSqlRepository
				.Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync(degree);

			mockSqlUnitOfWork
				.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockTransaction.Object);

			// Act
			var result = await handler.Handle(request, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			mockDegreeSqlRepository.Verify(repo => repo.Update(degree), Times.Once);
			mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
			mockTransaction.Verify(t => t.Commit(), Times.Once);
		}
		[Fact]
		public async Task Handle_DegreeNotFound_ThrowsCustomException()
		{
			// Arrange
			var request = new UpdateDegreeRequest { Id = 1, Name = "Updated Degree" };

			mockDegreeSqlRepository
				.Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync((Entities.Degree)null);

			// Act & Assert
			await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_ExceptionOccurs_TransactionRollsBack()
		{
			// Arrange
			var request = new UpdateDegreeRequest { Id = 1, Name = "Updated Degree" };
			var degree = new Entities.Degree { Id = 1, Name = "Old Degree" };
			var mockTransaction = new Mock<IDbTransaction>();

			mockDegreeSqlRepository
				.Setup(repo => repo.FindByIdAsync(request.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync(degree);

			mockSqlUnitOfWork
				.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockTransaction.Object);

			mockDegreeSqlRepository
				.Setup(repo => repo.Update(degree))
				.Throws(new Exception("Test Exception"));

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
			mockTransaction.Verify(t => t.Rollback(), Times.Once);
		}

		[Fact]
		public async Task Handle_InvalidRequest_ThrowsValidationException()
		{
			// Arrange
			var request = new UpdateDegreeRequest { Id = 0, Name = "" };
			var validator = new UpdateDegreeValidator();

			// Act & Assert
			Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));
		}
	}
}
