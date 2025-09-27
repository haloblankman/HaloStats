namespace HaloStats.Database.Entities;

public class Gamertag
{
    public Guid GamertagId { get; set; }
    public required string Name { get; set; }
    public required string XboxUserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Gamertag()
    {
        GamertagId = UUIDNext.Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
        CreatedAt = DateTime.UtcNow;
    }   
}
