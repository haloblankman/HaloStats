namespace HaloStats.Web.Shared.Contracts.Dashboard;

public class DashboardResponse
{
    public required TopPlayersByKillCount TopPlayersByKillCount { get; set; }
    public required TopPlayersByKillCount TopPlayersByKillCountThisMonth { get; set; }
    public required TopTeammatePairs TopTeammatePairs { get; set; }
    public required TopTeammatePairs TopTeammatePairsByWins { get; set; }
}
