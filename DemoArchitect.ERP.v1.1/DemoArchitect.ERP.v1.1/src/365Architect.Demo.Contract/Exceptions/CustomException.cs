using System.Net;
using _365Architect.Demo.Contract.Constants;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Enumerations;

namespace _365Architect.Demo.Contract.Exceptions
{
    /// <summary>
    ///     Provide application custom exception
    /// </summary>
    public class CustomException : Exception
    {
        /// <summary>
        ///     Indicate which exception happen, use same as <see cref="HttpStatusCode" />
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        ///     Indicate which message will be sent when exception happen
        /// </summary>
        public MsgCode MessageCode { get; set; }

        /// <summary>
        ///     Details of which causing exception
        /// </summary>
        public List<string> Details { get; set; } = new();

        /// <summary>
        /// Throw customize exception
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="msgCode"></param>
        /// <param name="messages"></param>
        /// <exception cref="CustomException"></exception>
        public static void ThrowException(int statusCode, MsgCode msgCode, params string[] messages)
        {
            throw new CustomException
            {
                StatusCode = statusCode,
                MessageCode = msgCode,
                Details = messages.ToList()
            };
        }

        /// <summary>
        /// Throw not found exception
        /// </summary>
        /// <exception cref="CustomException"></exception>
        public static void ThrowNotFoundException(Type? entityType = null, MsgCode msgCode = MsgCode.ERR_NF_FIND_KEY, string? message = null)
        {
            message ??= MsgConst.NOT_FOUND_FIND_KEY.FormatMsg(entityType?.Name ?? MsgConst.ENTITY);
            ThrowException((int)HttpStatusCode.NotFound, msgCode, message);
        }

        /// <summary>
        /// Throw conflict exception
        /// </summary>
        /// <exception cref="CustomException"></exception>
        public static void ThrowConflictException(MsgCode msgCode = MsgCode.ERR_CONFLICT, params string[] messages)
        {
            ThrowException((int)HttpStatusCode.Conflict, msgCode, messages);
        }

        /// <summary>
        /// Throw validation exception
        /// </summary>
        /// <param name="msgCode"></param>
        /// <param name="details"></param>
        public static void ThrowValidationException(MsgCode msgCode = MsgCode.ERR_INVALID, params string[] details)
        {
            ThrowException((int)HttpStatusCode.BadRequest, msgCode, details);
        }
    }
}