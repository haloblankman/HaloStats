namespace HaloStats.Web.Shared.Infrastructure;

public class PagedTableResponse<T>
{
    public required List<T> Records { get; set; }
    public required PagedTableRequest Parameters { get; set; }
    public required int TotalRecords { get; set; }
    public required bool IsNextPage { get; set; }
}
