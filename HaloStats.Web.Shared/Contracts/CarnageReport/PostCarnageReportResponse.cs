namespace HaloStats.Web.Shared.Contracts.CarnageReport;

public class PostCarnageReportResponse
{
    public Guid GameId { get; set; }
    public bool IsDuplicate { get; set; }
}
