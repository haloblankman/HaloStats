namespace HaloStats.Web.Shared.Contracts.Player;

public class GetPlayerResponse
{
    public required string Gamertag { get; set; }
    public required PlayerSummary PlayerSummary { get; set; }
    public required GamesPlayed GamesPlayed { get; set; }
}
