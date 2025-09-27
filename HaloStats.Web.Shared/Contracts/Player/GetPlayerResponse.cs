using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Shared.Contracts.Player;

public class GetPlayerResponse
{
    public required string Gamertag { get; set; }
    public required PlayerSummary PlayerSummary { get; set; }
    public required GetPlayerGamesResponse GamesPlayed { get; set; }
    public required TopTeammates TopTeammates { get; set; }
    public required TopTeammates TopTeammatesByWins { get; set; }
    public required TopOpponents TopOpponents { get; set; }
}
