using System.Net;
using Microsoft.Extensions.Hosting;
using ProtoResult;
using review.Contract.Enumerations;
using review.Contract.Exceptions;
using review.Contract.Helpers;
using review.Contract.Shared;
using Error = review.Contract.Errors.Error;

namespace review.Contract.DependencyInjection.Extensions
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