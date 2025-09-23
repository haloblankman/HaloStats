using UUIDNext;

namespace HaloStats.Database.Entities;

public class GameSeries
{
    public Guid GameSeriesId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }


    public GameSeries()
    {
        GameSeriesId = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
        CreatedAt = DateTime.UtcNow;
    }
}
