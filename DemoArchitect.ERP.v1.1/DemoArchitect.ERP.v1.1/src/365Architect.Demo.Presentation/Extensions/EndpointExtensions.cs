using _365Architect.Demo.Presentation.Services.Samples;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace _365Architect.Demo.Presentation.Extensions
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
            app.MapGrpcService<AdminSampleQueryGrpcService>().EnableGrpcWeb();
            app.MapGrpcService<UserSampleQueryGrpcService>().EnableGrpcWeb();

            // Sample command
            app.MapGrpcService<AdminSampleCommandGrpcService>().EnableGrpcWeb();
            app.MapGrpcService<UserSampleCommandGrpcService>().EnableGrpcWeb();

            app.MapGrpcReflectionService();
            return app;
        }
    }
}