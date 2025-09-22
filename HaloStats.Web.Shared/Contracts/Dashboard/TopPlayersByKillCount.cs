namespace HaloStats.Web.Shared.Contracts.Dashboard;
public class TopPlayersByKillCount
{
    public required List<PlayerByKillCount> Players { get; set; }
}

public class PlayerByKillCount
{
    public required int Rank { get; set; }
    public required string GamerTag { get; set; }
    public required int Kills { get; set; }
    public required int Deaths { get; set; }
    public required decimal KillDeathRatio { get; set; }
}
