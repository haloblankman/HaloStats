namespace HaloStats.Web.Shared.Contracts.Player;

public class GetPlayerResponse
{
    public required PlayerSummary PlayerSummary { get; set; }
    public required GamesPlayed GamesPlayed { get; set; }
}
