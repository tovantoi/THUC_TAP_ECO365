using System.Net;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Exceptions;
using _365Architect.Demo.Contract.Helpers;
using _365Architect.Demo.Contract.Shared;
using Microsoft.Extensions.Hosting;
using ProtoResult;
using Error = _365Architect.Demo.Contract.Errors.Error;

namespace _365Architect.Demo.Contract.DependencyInjection.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Convert <see cref="Exception"/> to <see cref="Result"/>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Result<object> ConvertToResult(this Exception exception)
        {
            // Check current environment
            bool isProduction = EnvironmentHelper.Environment.IsProduction();
            // Cast exception to custom exception
            CustomException? customException = exception as CustomException;
            // Convert exception to result
            return new Result<object>
            {
                MessageCode = customException?.MessageCode ?? MsgCode.ERR_INTERNAL_SERVER,
                StatusCode = customException?.StatusCode ?? (int)HttpStatusCode.InternalServerError,
                Error = isProduction ? null : new Error(exception.StackTrace ?? string.Empty, customException?.Details?.ToArray() ?? [exception.Message])
            };
        }

        /// <summary>
        /// Convert <see cref="Exception"/> to <see cref="ProtoResult.CommonResult"/>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static CommonResult ConvertToCommonResult(this Exception exception)
        {
            return exception.ConvertToResult().ConvertToCommonResult();
        }
    }
}