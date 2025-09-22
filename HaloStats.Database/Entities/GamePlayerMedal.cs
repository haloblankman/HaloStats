using UUIDNext;

namespace HaloStats.Database.Entities;

public class GamePlayerMedal
{
    public Guid GamePlayerMedalId { get; set; }
    public Guid GamePlayerId { get; set; }
    public required int MedalId { get; set; }
    public required int Count { get; set; }

    public GamePlayerMedal()
    {
        GamePlayerMedalId = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
    }
}
