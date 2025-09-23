using HaloStats.Database.Enums;
using HaloStats.Web.Shared.Enums;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class GameEnumMapper
{
    public static GameEnum MapToGameEnum(this HaloGames game)
    {
        return game switch
        {
            HaloGames.HaloCe => GameEnum.HaloCe,
            HaloGames.Halo2 => GameEnum.Halo2,
            HaloGames.Halo2Anniversary => GameEnum.Halo2Anniversary,
            HaloGames.Halo3 => GameEnum.Halo3,
            HaloGames.Halo3Odst => GameEnum.Halo3Odst,
            HaloGames.Halo4 => GameEnum.Halo4,
            HaloGames.HaloReach => GameEnum.HaloReach,
            _ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
        };
    }
}
