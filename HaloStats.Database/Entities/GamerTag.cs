namespace HaloStats.Database.Entities;

public class GamerTag
{
    public Guid GamerTagId { get; set; }
    public required string Name { get; set; }
    public required string XboxUserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public GamerTag()
    {
        GamerTagId = UUIDNext.Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
        CreatedAt = DateTime.UtcNow;
    }   
}
