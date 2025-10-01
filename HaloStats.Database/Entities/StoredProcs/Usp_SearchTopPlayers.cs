namespace HaloStats.Database.Entities.StoredProcs;

public class Usp_SearchTopPlayers
{
    public int Rank { get; set; }
    public string Gamertag { get; set; } = default!;
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public decimal KillDeathRatio { get; set; }
    public int TotalCount { get; set; }
}
