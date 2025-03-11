using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalWards
{
    /// <summary>
    /// Request to create a WebLocalWard, contains name, full name, coordinates, and district ID
    /// </summary>
    public record CreateWebLocalWardRequest : ICommand
    {
        /// <summary>
        /// Name of the ward
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// English name of the ward
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Full name of the ward
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Full English name of the ward
        /// </summary>
        public string FullNameEn { get; set; }

        /// <summary>
        /// Latitude coordinate of the ward
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude coordinate of the ward
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// ID of the district the ward belongs to
        /// </summary>
        public int DistrictId { get; set; }
    }
}
