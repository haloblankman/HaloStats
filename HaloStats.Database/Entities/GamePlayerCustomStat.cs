using UUIDNext;

namespace HaloStats.Database.Entities;

public class GamePlayerCustomStat
{
    public Guid GamePlayerCustomStatId { get; set; }
    public Guid GamePlayerId { get; set; }
    public required string StatName { get; set; }
    public required string Value { get; set; }

    public GamePlayerCustomStat()
    {
        GamePlayerCustomStatId = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
    }
}
