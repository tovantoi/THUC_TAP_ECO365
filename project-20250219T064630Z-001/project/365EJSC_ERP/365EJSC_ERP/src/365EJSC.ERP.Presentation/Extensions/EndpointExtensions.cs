using _365EJSC.ERP.Presentation.Services.Define.WebLocalWard;
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
            // Ward query
            app.MapGrpcService<WardQueryGrpcService>().EnableGrpcWeb();
            // Ward command
            app.MapGrpcService<WardCommandGrpcService>().EnableGrpcWeb();
            app.MapGrpcReflectionService();
            return app;
        }
    }
}