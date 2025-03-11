using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using _365EJSC.ERP.Application.UserCases.HRM.Degree;
using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Application.Validators.HRM.Degree;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using Entities=_365EJSC.ERP.Domain.Entities.HRM;
using System.Data;

namespace _365EJSC.ERP.Application.Tests.HRM.Degree
{
	public class CreateDegreeTest 
	{
		private readonly Mock<IDegreeSqlRepository> mockDegreeSqlRepository;
		private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
		private readonly CreateDegreeHandler handler;


		public CreateDegreeTest()
		{
			mockDegreeSqlRepository = new Mock<IDegreeSqlRepository>();
			mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
			handler = new CreateDegreeHandler(mockDegreeSqlRepository.Object, mockSqlUnitOfWork.Object);
		}
		[Fact]
		public Task Handle_InvalidRequest_ThrowsValidationException()
		{
			// Arrange
			var request = new CreateDegreeRequest
			{
				// Set invalid properties to trigger validation error
				Name = ""
			};

			var validator = new CreateDegreeValidator();
			Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

			// Act & Assert
			try
			{
				validator.ValidateAndThrow(request);
			}
			catch (CustomException e)
			{
				Assert.Equal(MsgCode.ERR_DEGREE_INVALID, e.MessageCode);
			}
			return Task.CompletedTask;
		}
		[Fact]
		public async Task Handle_RepositoryThrowsException_TransactionRollsBack()
		{
			// Arrange
			var request = new CreateDegreeRequest
			{
				Name = "Master of Science"
			};

			var mockTransaction = new Mock<IDbTransaction>();
			mockSqlUnitOfWork
				.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockTransaction.Object);

			mockDegreeSqlRepository
				.Setup(repo => repo.Add(It.IsAny<Entities.Degree>()))
				.Throws(new Exception());

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
			mockTransaction.Verify(t => t.Rollback(), Times.Once);
			mockTransaction.Verify(t => t.Commit(), Times.Never);
		}
		[Fact]
		public async Task Handle_ValidRequest_ReturnsSuccessResult()
		{
			// Arrange
			var request = new CreateDegreeRequest
			{
				Name = "Master of Science"
			};

			var mockTransaction = new Mock<IDbTransaction>();
			mockSqlUnitOfWork
				.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockTransaction.Object);

			mockDegreeSqlRepository
				.Setup(repo => repo.Add(It.IsAny<Entities.Degree>()));

			mockSqlUnitOfWork
				.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(It.IsAny<int>());

			// Act
			var result = await handler.Handle(request, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			mockDegreeSqlRepository.Verify(repo => repo.Add(It.IsAny<Entities.Degree>()), Times.Once);
			mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
			mockTransaction.Verify(t => t.Commit(), Times.Once);
			mockTransaction.Verify(t => t.Rollback(), Times.Never);
		}
	}
}
