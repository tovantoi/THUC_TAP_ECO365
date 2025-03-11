using Microsoft.AspNetCore.Hosting;

namespace _365EJSC.ERP.Contract.Helpers
{
    /// <summary>
    /// Static helper class provide <see cref="IWebHostEnvironment"/>
    /// </summary>
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Web environment
        /// </summary>
        public static IWebHostEnvironment Environment { get; set; }
    }
}