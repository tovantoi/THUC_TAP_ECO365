using _365EJSC.ERP.Presentation.Services.Define;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace _365EJSC.ERP.Presentation.Extensions
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
            app.MapGrpcService<WebLocalWardGrpcService>().EnableGrpcWeb();
            app.MapGrpcReflectionService();
            return app;
        }
    }
}