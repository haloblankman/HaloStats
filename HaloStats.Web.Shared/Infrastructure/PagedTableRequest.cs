namespace HaloStats.Web.Shared.Infrastructure;

public class PagedTableRequest
{
    public required string? SortedBy { get; set; }
    public required SortDirection SortDirection { get; set; }
    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }

}

public enum SortDirection
{
    None,
    Ascending,
    Descending
}
