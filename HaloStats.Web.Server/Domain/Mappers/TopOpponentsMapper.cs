using HaloStats.Database.Entities.StoredProcs;
using HaloStats.Web.Shared.Contracts.Shared;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class TopOpponentsMapper
{
    public static TopOpponentRecord ToTopOpponentRecord(this Usp_GetTopOpponentPairs entity, int rank)
    {
        return new TopOpponentRecord
        {
            Rank = rank,
            OpponentGamertag = entity.OpponentGamertag,
            GamesPlayedAgainst = entity.GamesPlayedAgainst,
            WinsAgainst = entity.WinsAgainst,
            LossesAgainst = entity.LossesAgainst,
            WinRate = entity.WinRate
        };
    }
}

