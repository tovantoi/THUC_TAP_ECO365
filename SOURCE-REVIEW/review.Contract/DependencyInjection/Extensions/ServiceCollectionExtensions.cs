using Microsoft.AspNetCore.Hosting;
using review.Contract.Helpers;

namespace review.Contract.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="EnvironmentHelper"/>, have easily way to get <see cref="IWebHostEnvironment"/>
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IWebHostEnvironment AddEnvironmentHelper(this IWebHostEnvironment env)
        {
            EnvironmentHelper.Environment = env;
            return env;
        }
    }
}