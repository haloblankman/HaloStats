namespace HaloStats.Web.Shared.Infrastructure;

public class PagedTableRequest
{
    public required string? SortedBy { get; set; }
    public required SortDirection SortDirection { get; set; }
    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }

    public int PageSizePlusOne => PageSize + 1;
    public string SortDirectionStringDefaultDesc => SortDirection is SortDirection.None or SortDirection.Descending ? "DESC" : "ASC";
    public string SortDirectionStringDefaultAsc => SortDirection is SortDirection.None or SortDirection.Ascending ? "ASC" : "DESC";
}

public enum SortDirection
{
    None,
    Ascending,
    Descending
}
