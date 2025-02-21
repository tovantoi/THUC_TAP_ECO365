using Microsoft.Extensions.DependencyInjection;

namespace review.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register presentation services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddGrpc();
            services.AddGrpcReflection();
            return services;
        }
    }
}