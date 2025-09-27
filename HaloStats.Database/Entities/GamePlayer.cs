using UUIDNext;

namespace HaloStats.Database.Entities;

public class GamePlayer
{
    public Guid GamePlayerId { get; set; }
    public Guid GameId { get; set; }
    public required string XboxUserId { get; set; }
    public required bool IsGuest { get; set; }
    public bool IsWinner { get; set; }
    public int Place { get; set; }
    public required int GameMode { get; set; }
    public required string Gamertag { get; set; }
    public required string ClanTag { get; set; }
    public required int EmblemTexture0 { get; set; }
    public required int EmblemTexture1 { get; set; }
    public required int EmblemColor0 { get; set; }
    public required int EmblemColor1 { get; set; }
    public required int Nameplate { get; set; }
    public required int Avatar { get; set; }
    public required string ServiceId { get; set; }
    public required int TeamId { get; set; }
    public required int Score { get; set; }
    public required int Standing { get; set; }
    public required int TotalMedalCount { get; set; }
    public required int Kills { get; set; }
    public required int Deaths { get; set; }
    public required int Assists { get; set; }
    public required int Betrayals { get; set; }
    public required int Suicides { get; set; }
    public required int MostKillsInARow { get; set; }
    public required int SecondsAlive { get; set; }
    public required int KillsWeapon { get; set; }
    public required int KillsGrenade { get; set; }
    public required int KillsMelee { get; set; }
    public required int KillsOther { get; set; }
    public required int CompletedGame { get; set; }
    public required int SecondsPlayed { get; set; }
    public required int KilledMostPlayerIndex { get; set; }
    public required int KilledMostPlayerCount { get; set; }
    public required int MostKilledByPlayerIndex { get; set; }
    public required int MostKilledByPlayerCount { get; set; }
    public required int MostUsedWeapon { get; set; }
    public required int MostUsedWeaponCount { get; set; }

    public List<GamePlayerCustomStat>? CustomStats { get; set; }
    public List<GamePlayerMedal>? Medals { get; set; }

    public GamePlayer()
    {
        GamePlayerId = Uuid.NewDatabaseFriendly(UUIDNext.Database.PostgreSql);
    }
}
