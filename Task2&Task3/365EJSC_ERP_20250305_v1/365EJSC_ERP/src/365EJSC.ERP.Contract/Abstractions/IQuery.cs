using _365EJSC.ERP.Contract.Shared;
using MediatR;

namespace _365EJSC.ERP.Contract.Abstractions
{
    /// <summary>
    /// Define a query with data return as format of <see cref="Result"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuery<T> : IRequest<Result<T>> where T : class
    {
    }
}