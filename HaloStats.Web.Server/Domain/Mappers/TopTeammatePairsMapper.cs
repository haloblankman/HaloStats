using HaloStats.Database.Entities.StoredProcs;
using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class TopTeammatePairsMapper
{
    public static TeammatePair ToTopTeammatePairs(this Usp_GetTopTeammatePairs entity, int rank)
    {
        return new TeammatePair
        {
            Rank = 0,
            PlayerOneGamerTag = entity.PlayerOneGamerTag,
            PlayerTwoGamerTag = entity.PlayerTwoGamerTag,
            GamesPlayedTogether = entity.GamesPlayedTogether,
            WinsTogether = entity.WinsTogether,
            LossesTogether = entity.LossesTogether,
            WinRate = entity.WinRate
        };
    }
}
