using MediatR;
using review.Contract.Shared;

namespace review.Contract.Abstractions
{
    /// <summary>
    /// Define a command with returned as format of <see cref="Result"/> without data
    /// </summary>
    public interface ICommand : IRequest<Result<object>>
    {
    }

    /// <summary>
    /// Define a command with returned as format of <see cref="Result{TModel}"/> with data
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface ICommand<T> : IRequest<Result<T>> where T : class
    {
    }
}