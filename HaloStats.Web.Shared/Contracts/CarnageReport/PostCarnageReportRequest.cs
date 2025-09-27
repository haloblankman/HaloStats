namespace HaloStats.Web.Shared.Contracts.CarnageReport;

public class PostCarnageReportRequest
{
    public Guid GameUniqueId { get; set; }
    public string GameTypeName { get; set; }
    public bool IsMatchmaking { get; set; }
    public bool LastMatchIncomplete { get; set; }
    public bool IsTeamsEnabled { get; set; }
    public uint HopperId { get; set; }
    public string HopperName { get; set; }
    public int PartySize { get; set; }
    public bool HasNetworkMembersInParty { get; set; }
    public int GameEnum { get; set; }
    public List<GamePlayerInfo> Players { get; set; }
}

public class GamePlayerInfo
{
    public string XboxUserId { get; set; }
    public bool IsGuest { get; set; }
    public int GameMode { get; set; }
    public string Gamertag { get; set; }
    public string ClanTag { get; set; }
    public int EmblemTexture0 { get; set; }
    public int EmblemTexture1 { get; set; }
    public int EmblemColor0 { get; set; }
    public int EmblemColor1 { get; set; }
    public int Nameplate { get; set; }
    public int Avatar { get; set; }
    public string ServiceId { get; set; }
    public int TeamId { get; set; }
    public int Score { get; set; }
    public int Standing { get; set; }
    public int TotalMedalCount { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public int Betrayals { get; set; }
    public int Suicides { get; set; }
    public int MostKillsInARow { get; set; }
    public int SecondsAlive { get; set; }
    public int KillsWeapon { get; set; }
    public int KillsGrenade { get; set; }
    public int KillsMelee { get; set; }
    public int KillsOther { get; set; }
    public int CompletedGame { get; set; }
    public int SecondsPlayed { get; set; }
    public int KilledMostPlayerIndex { get; set; }
    public int KilledMostPlayerCount { get; set; }
    public int MostKilledByPlayerIndex { get; set; }
    public int MostKilledByPlayerCount { get; set; }
    public int MostUsedWeapon { get; set; }
    public int MostUsedWeaponCount { get; set; }

    public List<GamePlayerCustomStatInfo> CustomStats { get; set; }
    public List<GamePlayerMedalInfo> Medals { get; set; }
}

public class GamePlayerCustomStatInfo
{
    public string StatName { get; set; }
    public string Value { get; set; }
}

public class GamePlayerMedalInfo
{
    public int MedalId { get; set; }
    public int Count { get; set; }
}
