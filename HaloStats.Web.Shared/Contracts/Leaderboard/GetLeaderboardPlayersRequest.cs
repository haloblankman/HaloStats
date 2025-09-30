using HaloStats.Web.Shared.Infrastructure;

namespace HaloStats.Web.Shared.Contracts.Leaderboard;

public class GetLeaderboardPlayersRequest : PagedTableRequest
{
    public bool IsMonthly { get; set; }
    public string? SearchGamertag { get; set; }

    
}
