using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Shared.Contracts.Leaderboard;

public class GetLeaderboardRequest
{
    public HaloGames Game { get; set; }
}
