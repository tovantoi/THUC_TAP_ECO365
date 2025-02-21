using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using review.Presentation.Services.Product;

namespace review.Presentation.Extensions
{
    public static class EndpointExtensions
    {
        /// <summary>
        /// Map presentation endpoint
        /// </summary>
        /// <param name="app">Endpoint route builder</param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapPresentationEndpoint(this IEndpointRouteBuilder app)
        {
            // Sample query
            app.MapGrpcService<AdminProductQueryGrpcService>().EnableGrpcWeb();
            app.MapGrpcService<UserProductQueryGrpcService>().EnableGrpcWeb();

            // Sample command
            app.MapGrpcService<AdminProductCommandGrpcService>().EnableGrpcWeb();
            app.MapGrpcService<UserProductCommandGrpcService>().EnableGrpcWeb();

            app.MapGrpcReflectionService();
            return app;
        }
    }
}