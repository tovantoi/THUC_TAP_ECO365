using _365EJSC.ERP.Contract.Constants;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Persistence.Repositories;
using _365EJSC.ERP.Persistence.Repositories.Base;
using _365EJSC.ERP.Persistence.Repositories.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _365EJSC.ERP.Persistence.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register infrastructure EF services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString(Const.CONN_CONFIG_SQL);
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.RegisterServices();
            return services;
        }

        /// <summary>
        /// Registering infrastructure ef services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericSqlRepository<,>), typeof(GenericSqlRepository<,>));
            services.AddScoped<ISqlUnitOfWork, SqlUnitOfWork>();
            services.AddScoped<IWebLocalSqlRepository, WebLocalSqlRepository>();
            services.AddScoped<IWebLocalProvinceSqlRepository, WebLocalProvinceSqlRepository>();
            services.AddScoped<IWebLocalDistrictSqlRepository, WebLocalDistrictSqlRepository>();
            services.AddScoped<IWebLocalWardSqlRepository, WebLocalWardSqlRepository>();
            services.AddScoped<ICompanySqlRepository, CompanySqlRepository>();
            services.AddScoped<ICompanyDepartmentSqlRepository, CompanyDepartmentSqlRepository>();
            services.AddScoped<IGeneralDepartmentSqlRepository, GeneralDepartmentSqlRepository>();
            services.AddScoped<IErpGeneralPositionSqlRepository, ErpGeneralPositionSqlRepository>();
            services.AddScoped<IMaritalSqlRepository, MaritalSqlRepository>();
            services.AddScoped<IEmployeeRoleSqlRepository, EmployeeRoleSqlRepository>();
            services.AddScoped<IBankSqlRepository, BankSqlRepository>();
            return services;
        }
    }
}