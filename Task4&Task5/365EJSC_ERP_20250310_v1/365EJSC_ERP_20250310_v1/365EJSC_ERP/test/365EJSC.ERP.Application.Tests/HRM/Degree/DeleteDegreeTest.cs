using _365EJSC.ERP.Application.UserCases.HRM.Degree;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using _365EJSC.ERP.Application.Requests.HRM.Degree;
using System.Data;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;

namespace _365EJSC.ERP.Application.Tests.HRM.Degree
{
	public class DeleteDegreeTest
	{
		private readonly Mock<IDegreeSqlRepository> mockDegreeSqlRepository;
		private readonly Mock<ISqlUnitOfWork> mockSqlUnitOfWork;
		private readonly DeleteDegreeHandler handler;
		public DeleteDegreeTest()
		{
			mockDegreeSqlRepository = new Mock<IDegreeSqlRepository>();
			mockSqlUnitOfWork = new Mock<ISqlUnitOfWork>();
			handler = new DeleteDegreeHandler(mockDegreeSqlRepository.Object, mockSqlUnitOfWork.Object);
		}
		[Fact]
		public async Task Handle_ValidRequest_ReturnsSuccessResult()
		{
			// Arrange
			var request = new DeleteDegreeRequest { Id = 1 };
			var degree = new Entities.Degree
			{
				Id = 1
			};

			var mockTransaction = new Mock<IDbTransaction>();

			mockDegreeSqlRepository
				.Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync(degree);

			mockSqlUnitOfWork
				.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockTransaction.Object);

			mockSqlUnitOfWork
				.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(1);

			// Act
			var result = await handler.Handle(request, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			mockDegreeSqlRepository.Verify(repo => repo.Remove(It.IsAny<Entities.Degree>()), Times.Once);
			mockSqlUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
			mockTransaction.Verify(t => t.Commit(), Times.Once);
			mockTransaction.Verify(t => t.Rollback(), Times.Never);
		}
		[Fact]
		public async Task Handle_DegreeNotFound_ThrowsCustomException()
		{
			// Arrange
			var request = new DeleteDegreeRequest { Id = 1 };

			mockDegreeSqlRepository
				.Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
				.ThrowsAsync(new CustomException
				{
					MessageCode = MsgCode.ERR_DEGREE_ID_NOT_FOUND
				});

			// Act & Assert
			await Assert.ThrowsAsync<CustomException>(() => handler.Handle(request, CancellationToken.None));
			try
			{
				await handler.Handle(request, CancellationToken.None);
			}
			catch (CustomException e)
			{
				Assert.Equal(MsgCode.ERR_DEGREE_ID_NOT_FOUND, e.MessageCode);
			}
		}
		[Fact]
		public async Task Handle_ExceptionOccurs_TransactionRollsBack()
		{
			// Arrange
			var request = new DeleteDegreeRequest { Id = 1 };
			var degree = new Entities.Degree();
			var mockTransaction = new Mock<IDbTransaction>();

			mockDegreeSqlRepository
				.Setup(repo => repo.FindByIdAsync((int)request.Id, true, It.IsAny<CancellationToken>()))
				.ReturnsAsync(degree);

			mockSqlUnitOfWork
				.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(mockTransaction.Object);

			mockDegreeSqlRepository
				.Setup(repo => repo.Remove(It.IsAny<Entities.Degree>()))
				.Throws(new Exception("Test exception"));

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
			mockTransaction.Verify(t => t.Rollback(), Times.Once);
			mockTransaction.Verify(t => t.Commit(), Times.Never);
		}
	}
}
