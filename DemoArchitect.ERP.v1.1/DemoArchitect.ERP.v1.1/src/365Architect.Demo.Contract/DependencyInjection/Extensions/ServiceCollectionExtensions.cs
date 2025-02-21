using _365Architect.Demo.Contract.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace _365Architect.Demo.Contract.DependencyInjection.Extensions
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