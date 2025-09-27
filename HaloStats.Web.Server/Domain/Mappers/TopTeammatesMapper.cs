using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class TopTeammatesMapper
{
    public static TopTeammates ToTopTeammates(this TopTeammatePairs topPairs)
    {
        return new TopTeammates
        {
            Teammates = topPairs.Pairs.ConvertAll(ToTopTeammateRecord)
        };
    }

    private static TopTeammateRecord ToTopTeammateRecord(this TeammatePair pair)
    {
        return new TopTeammateRecord
        {
            GamesPlayedTogether = pair.GamesPlayedTogether,
            LossesTogether = pair.LossesTogether,
            Rank = pair.Rank,
            TeammateGamertag = pair.PlayerTwoGamertag,
            WinRate = pair.WinRate,
            WinsTogether = pair.WinsTogether
        };
    }
}
