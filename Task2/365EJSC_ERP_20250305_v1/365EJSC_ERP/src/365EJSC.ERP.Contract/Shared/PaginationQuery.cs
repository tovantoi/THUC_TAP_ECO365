namespace _365EJSC.ERP.Contract.Shared
{
    public class PaginationQuery
    {
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationQuery()
        {
        }

        public PaginationQuery(int pageNumber, int pageSize, string? sortBy = null, bool? isDescending = null)
        {
            SortBy = sortBy;
            IsDescending = isDescending ?? true;
            PageNumber = Math.Max(pageNumber, 1);
            PageSize = pageSize;
        }
    }
}