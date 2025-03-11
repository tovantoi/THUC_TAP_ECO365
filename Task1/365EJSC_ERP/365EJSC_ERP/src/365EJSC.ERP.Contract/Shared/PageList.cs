namespace _365EJSC.ERP.Contract.Shared
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public PagedList(List<T> items, int totalItems, int? pageNumber = null, int? pageSize = null)
        {
            Items = items;
            TotalItems = totalItems;
            PageNumber = Math.Max(pageNumber ?? 1, 1);
            PageSize = Math.Max(pageSize ?? 10, 1);
        }

        public static implicit operator List<T>(PagedList<T> pagedList) => pagedList.Items;
        public static implicit operator PagedList<T>(List<T> itemList) => new(itemList, itemList.Count);
    }
}