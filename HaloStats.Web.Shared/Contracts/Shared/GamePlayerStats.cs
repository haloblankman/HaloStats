namespace HaloStats.Web.Shared.Contracts.Shared;

public class GamePlayerStats
{
    public required string ClanTag { get; set; }
    public required string GamerTag { get; set; }
    public required int Score { get; set; }
    public required int Kills { get; set; }
    public required int Assists { get; set; }
    public required int Deaths { get; set; }
    public required int Place { get; set; }
    public required int WeaponKills { get; set; }
    public required int GrenadeKills { get; set; }
    public required int MeleeKills { get; set; }
    public required int OtherKills { get; set; }
    public required int SecondsAlive { get; set; }
    public required int KillDeathSpread { get; set; }
    public required int Spree { get; set; }
    public required int Medals { get; set; }
    public required int TeamId { get; set; }
}
