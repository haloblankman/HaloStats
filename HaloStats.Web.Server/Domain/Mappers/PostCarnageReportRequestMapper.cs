using HaloStats.Database.Entities;
using HaloStats.Web.Shared.Contracts.CarnageReport;

namespace HaloStats.Web.Server.Domain.Mappers;

public static class PostCarnageReportRequestMapper
{
    public static Game MapToGame(PostCarnageReportRequest request, string ipAddress)
    {
        return new Game()
        {
            GameUniqueId = request.GameUniqueId,
            WhosReportingIp = ipAddress,
            GameTypeName = request.GameTypeName,
            IsMatchmaking = request.IsMatchmaking,
            LastMatchIncomplete = request.LastMatchIncomplete,
            IsTeamsEnabled = request.IsTeamsEnabled,
            HopperId = request.HopperId,
            HopperName = request.HopperName,
            PartySize = request.PartySize,
            HasNetworkMembersInParty = request.HasNetworkMembersInParty,
            GameEnum = request.GameEnum,
            Players = request.Players?.Select(p => new GamePlayer
            {
                XboxUserId = p.XboxUserId,
                IsGuest = p.IsGuest,
                GameMode = p.GameMode,
                GamerTag = p.GamerTag,
                ClanTag = p.ClanTag,
                EmblemTexture0 = p.EmblemTexture0,
                EmblemTexture1 = p.EmblemTexture1,
                EmblemColor0 = p.EmblemColor0,
                EmblemColor1 = p.EmblemColor1,
                Nameplate = p.Nameplate,
                Avatar = p.Avatar,
                ServiceId = p.ServiceId,
                TeamId = p.TeamId,
                Score = p.Score,
                Standing = p.Standing,
                TotalMedalCount = p.TotalMedalCount,
                Kills = p.Kills,
                Deaths = p.Deaths,
                Assists = p.Assists,
                Betrayals = p.Betrayals,
                Suicides = p.Suicides,
                MostKillsInARow = p.MostKillsInARow,
                SecondsAlive = p.SecondsAlive,
                KillsWeapon = p.KillsWeapon,
                KillsGrenade = p.KillsGrenade,
                KillsMelee = p.KillsMelee,
                KillsOther = p.KillsOther,
                CompletedGame = p.CompletedGame,
                SecondsPlayed = p.SecondsPlayed,
                KilledMostPlayerIndex = p.KilledMostPlayerIndex,
                KilledMostPlayerCount = p.KilledMostPlayerCount,
                MostKilledByPlayerIndex = p.MostKilledByPlayerIndex,
                MostKilledByPlayerCount = p.MostKilledByPlayerCount,
                MostUsedWeapon = p.MostUsedWeapon,
                MostUsedWeaponCount = p.MostUsedWeaponCount,
                CustomStats = p.CustomStats?.Select(cs => new GamePlayerCustomStat
                {
                    StatName = cs.StatName,
                    Value = cs.Value
                }).ToList() ?? new List<GamePlayerCustomStat>(),
                Medals = p.Medals?.Select(m => new GamePlayerMedal
                {
                    MedalId = m.MedalId,
                    Count = m.Count
                }).ToList() ?? new List<GamePlayerMedal>()
            }).ToList() ?? new List<GamePlayer>()
        };
    }
}
