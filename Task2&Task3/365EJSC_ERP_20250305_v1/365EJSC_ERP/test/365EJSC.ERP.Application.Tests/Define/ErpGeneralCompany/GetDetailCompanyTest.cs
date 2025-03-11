using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.UserCases.Define.ErpGeneralCompany;
using _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Moq;
using Entities = _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.Tests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Test class for GetDetailCompanyHandler.
    /// </summary>
    public class GetDetailCompanyTest
    {
        private readonly Mock<ICompanySqlRepository> mockCompanyRepository;
        private readonly GetDetailCompanyHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDetailCompanyTest"/> class.
        /// </summary>
        public GetDetailCompanyTest()
        {
            mockCompanyRepository = new Mock<ICompanySqlRepository>();
            handler = new GetDetailCompanyHandler(mockCompanyRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnCompany_When_Found()
        {
            // Arrange
            var company = new Entities.ErpGeneralCompany
            {
                CompanyPid = 1,
                TaxCode = "1234567890",
                Name = "Company 1",
                Image = "company1.png",
                Tel = "123-456-7890",
                Email = "contact@company1.com",
                Website = "www.company1.com",
                Founder = "Founder 1",
                Ceo = "CEO 1",
                CeoImage = "ceo1.png",
                CeoEmail = "ceo1@company1.com",
                CeoTel = "098-765-4321",
                License = "ABC123456",
                CountryId = "AAA",
                WardId = 1,
                IsActived = true
            };

            mockCompanyRepository.Setup(r => r.FindByIdAsync(1, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);
            var request = new GetDetailCompanyRequest { Id = 1 };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(company, result.Data);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_Company_NotFound()
        {
            // Arrange
            mockCompanyRepository.Setup(r => r.FindByIdAsync(99, false, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CustomException
                {
                    MessageCode = MsgCode.ERR_COMPANY_ID_NOT_FOUND,
                });
            var request = new GetDetailCompanyRequest { Id = 99 };

            // Act
            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<CustomException>(act);
            try
            {
                await handler.Handle(request, CancellationToken.None);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_COMPANY_ID_NOT_FOUND, e.MessageCode);
            }
        }

        [Fact]
        public Task Handle_Should_ThrowException_When_Request_Invalid()
        {
            // Arrange
            var request = new GetDetailCompanyRequest
            {
                // Set invalid properties of CreateCompanyCommand here
            };

            var validator = new GetDetailCompanyValidator();
            Assert.Throws<CustomException>(() => validator.ValidateAndThrow(request));

            // Act & Assert
            try
            {
                validator.ValidateAndThrow(request);
            }
            catch (CustomException e)
            {
                Assert.Equal(MsgCode.ERR_COMPANY_INVALID, e.MessageCode);
            }
            return Task.CompletedTask;
        }
    }
}