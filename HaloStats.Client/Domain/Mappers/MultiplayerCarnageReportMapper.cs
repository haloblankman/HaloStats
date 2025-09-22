using HaloStats.Web.Shared.Contracts.CarnageReport;

namespace HaloStats.Client.Domain.Mappers;

public static class MultiplayerCarnageReportMapper
{
    public static PostCarnageReportRequest MapToPostRequest(MultiplayerCarnageReport report)
    {
        return new PostCarnageReportRequest
        {
            GameUniqueId = report.GameUniqueId?.Value ?? Guid.Empty,
            GameTypeName = report.GameTypeName?.Value ?? string.Empty,
            IsMatchmaking = report.IsMatchmaking?.Value ?? false,
            LastMatchIncomplete = report.LastMatchIncomplete?.Value ?? false,
            IsTeamsEnabled = report.IsTeamsEnabled?.Value ?? false,
            HopperId = report.HopperId?.Value ?? 0,
            HopperName = report.HopperName?.Value ?? string.Empty,
            PartySize = report.PartySize?.Value ?? 0,
            HasNetworkMembersInParty = report.HasNetworkMembersInParty?.Value ?? false,
            GameEnum = report.GameEnum?.Value ?? 0,
            Players = report.Players?.Select(p => new GamePlayerInfo
            {
                XboxUserId = p.mXboxUserId,
                IsGuest = p.isGuest,
                GameMode = p.mGameMode,
                GamerTag = p.mGamertagText,
                ClanTag = p.ClantagText,
                EmblemTexture0 = p.EmblemTexture0,
                EmblemTexture1 = p.EmblemTexture1,
                EmblemColor0 = p.EmblemColor0,
                EmblemColor1 = p.EmblemColor1,
                Nameplate = p.Nameplate,
                Avatar = p.Avatar,
                ServiceId = p.ServiceId,
                TeamId = p.mTeamId,
                Score = p.Score,
                Standing = p.mStanding,
                TotalMedalCount = p.mTotalMedalCount,
                Kills = p.mKills,
                Deaths = p.mDeaths,
                Assists = p.mAssists,
                Betrayals = p.mBetrayals,
                Suicides = p.mSuicides,
                MostKillsInARow = p.mMostKillsInARow,
                SecondsAlive = p.mSecondsAlive,
                KillsWeapon = p.mKillsWeapon,
                KillsGrenade = p.mKillsGrenade,
                KillsMelee = p.mKillsMelee,
                KillsOther = p.mKillsOther,
                CompletedGame = p.mCompletedGame,
                SecondsPlayed = p.mSecondsPlayed,
                KilledMostPlayerIndex = p.mKilledMostPlayerIndex,
                KilledMostPlayerCount = p.mKilledMostPlayerCount,
                MostKilledByPlayerIndex = p.mMostKilledByPlayerIndex,
                MostKilledByPlayerCount = p.mMostKilledByPlayerCount,
                MostUsedWeapon = p.mMostUsedWeapon,
                MostUsedWeaponCount = p.mMostUsedWeaponCount,
                CustomStats = p.CustomStats?.Stats?
                .Where(cs => !string.IsNullOrEmpty(cs.mStatName))
                .Select(cs => new GamePlayerCustomStatInfo
                {
                    StatName = cs.mStatName,
                    Value = cs.mValueForDisplay
                }).ToList() ?? new List<GamePlayerCustomStatInfo>(),
                Medals = p.MedalsCount?.Medals?
                .Where(m => m.mCount > 0)
                .Select(m => new GamePlayerMedalInfo
                {
                    MedalId = m.mId,
                    Count = m.mCount
                }).ToList() ?? new List<GamePlayerMedalInfo>()
            }).ToList() ?? new List<GamePlayerInfo>()
        };
    }
}
