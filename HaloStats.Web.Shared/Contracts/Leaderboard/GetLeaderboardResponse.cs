using HaloStats.Web.Shared.Contracts.Shared;
using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Shared.Contracts.Leaderboard;

public class GetLeaderboardResponse
{
    public required HaloGames Game { get; set; }
    public required GetLeaderboardPlayersResponse TopPlayersThisMonth { get; set; }
    public required TopTeammatePairs TopTeammatePairs { get; set; }
    public required TopTeammatePairs TopTeammatePairsByWins { get; set; }
}
