namespace TaskManager.API.DTO.Response
{
    public class PaginatedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Items { get; set; }
    }
}
