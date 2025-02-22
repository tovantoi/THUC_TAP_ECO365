using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWards
{
    /// <summary>
    /// Request to delete a WebLocalWard by its ID
    /// </summary>
    public record DeleteWebLocalWardRequest : ICommand
    {
        /// <summary>
        /// ID of the ward to be deleted
        /// </summary>
        public int? Id { get; set; }
    }
}
