namespace HaloStats.Web.Shared.Contracts.Shared;

public class GamerTagInfo
{
    public required Guid GamerTagId { get; set; }
    public required string Name { get; set; }
    public required string XboxUserId { get; set; }
}
