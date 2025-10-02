using HaloStats.Web.Shared.Infrastructure;

namespace HaloStats.Web.Shared.Contracts.Leaderboard;

public class GetLeaderboardTeammatePairsRequest : PagedTableRequest
{
    public bool? IsMonthly { get; set; }
    public bool? IsMatchmaking { get; set; }
    public bool? IsCustoms { get; set; }
    public string? SearchGamertag { get; set; }
}
