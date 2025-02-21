using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace _365Architect.Demo.Application.DependencyInjection.Extension
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register application services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register mediator
            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
            return services;
        }
    }
}