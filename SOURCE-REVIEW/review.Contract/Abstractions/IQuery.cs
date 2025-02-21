using MediatR;
using review.Contract.Shared;

namespace review.Contract.Abstractions
{
    /// <summary>
    /// Define a query with data return as format of <see cref="Result"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuery<T> : IRequest<Result<T>> where T : class
    {
    }
}