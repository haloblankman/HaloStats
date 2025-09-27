namespace HaloStats.Web.Shared.Contracts.Shared;

public class GamertagInfo
{
    public required Guid GamertagId { get; set; }
    public required string Name { get; set; }
    public required string XboxUserId { get; set; }
}
