using System.Xml.Serialization;

[XmlRoot("MultiplayerCarnageReport")]
public class MultiplayerCarnageReport
{
    public GameEnum GameEnum { get; set; }
    public IsMatchmaking IsMatchmaking { get; set; }
    public HasNetworkMembersInParty HasNetworkMembersInParty { get; set; }
    public PartySize PartySize { get; set; }
    public LastMatchIncomplete LastMatchIncomplete { get; set; }
    public IsTeamsEnabled IsTeamsEnabled { get; set; }
    public HopperId HopperId { get; set; }
    public HopperName HopperName { get; set; }
    public GameTypeName GameTypeName { get; set; }
    public GameUniqueId GameUniqueId { get; set; }

    [XmlArray("Players")]
    [XmlArrayItem("Player")]
    public List<Player> Players { get; set; }
}

public class GameEnum
{
    [XmlAttribute("mGameEnum")]
    public int Value { get; set; }
}

public class IsMatchmaking
{
    [XmlAttribute("IsMatchmaking")]
    public bool Value { get; set; }
}

public class HasNetworkMembersInParty
{
    [XmlAttribute("mHasNetworkMembersInParty")]
    public bool Value { get; set; }
}

public class PartySize
{
    [XmlAttribute("mPartySize")]
    public int Value { get; set; }
}

public class LastMatchIncomplete
{
    [XmlAttribute("mLastMatchIncomplete")]
    public bool Value { get; set; }
}

public class IsTeamsEnabled
{
    [XmlAttribute("IsTeamsEnabled")]
    public bool Value { get; set; }
}

public class HopperId
{
    [XmlAttribute("HopperId")]
    public uint Value { get; set; }
}

public class HopperName
{
    [XmlAttribute("HopperName")]
    public string Value { get; set; }
}

public class GameTypeName
{
    [XmlAttribute("GameTypeName")]
    public string Value { get; set; }
}

public class GameUniqueId
{
    [XmlAttribute("GameUniqueId")]
    public Guid Value { get; set; }
}

public class Player
{
    [XmlAttribute] public string mXboxUserId { get; set; }
    [XmlAttribute] public bool isGuest { get; set; }
    [XmlAttribute] public int mGameMode { get; set; }
    [XmlAttribute] public string mGamertagText { get; set; }
    [XmlAttribute] public string ClantagText { get; set; }
    [XmlAttribute] public int EmblemTexture0 { get; set; }
    [XmlAttribute] public int EmblemTexture1 { get; set; }
    [XmlAttribute] public int EmblemColor0 { get; set; }
    [XmlAttribute] public int EmblemColor1 { get; set; }
    [XmlAttribute] public int EmblemColor2 { get; set; }
    [XmlAttribute] public int Nameplate { get; set; }
    [XmlAttribute] public int Avatar { get; set; }
    [XmlAttribute] public string ServiceId { get; set; }
    [XmlAttribute] public int mTeamId { get; set; }
    [XmlAttribute] public int Score { get; set; }
    [XmlAttribute] public int mStanding { get; set; }
    [XmlAttribute] public int mTotalMedalCount { get; set; }
    [XmlAttribute] public int mKills { get; set; }
    [XmlAttribute] public int mDeaths { get; set; }
    [XmlAttribute] public int mAssists { get; set; }
    [XmlAttribute] public int mBetrayals { get; set; }
    [XmlAttribute] public int mSuicides { get; set; }
    [XmlAttribute] public int mMostKillsInARow { get; set; }
    [XmlAttribute] public int mSecondsAlive { get; set; }
    [XmlAttribute] public int mKillsWeapon { get; set; }
    [XmlAttribute] public int mKillsGrenade { get; set; }
    [XmlAttribute] public int mKillsMelee { get; set; }
    [XmlAttribute] public int mKillsOther { get; set; }
    [XmlAttribute] public int mCompletedGame { get; set; }
    [XmlAttribute] public int mSecondsPlayed { get; set; }
    [XmlAttribute] public int mKilledMostPlayerIndex { get; set; }
    [XmlAttribute] public int mKilledMostPlayerCount { get; set; }
    [XmlAttribute] public int mMostKilledByPlayerIndex { get; set; }
    [XmlAttribute] public int mMostKilledByPlayerCount { get; set; }
    [XmlAttribute] public int mMostUsedWeapon { get; set; }
    [XmlAttribute] public int mMostUsedWeaponCount { get; set; }

    public CustomStats CustomStats { get; set; }
    public MedalsCount MedalsCount { get; set; }
}

public class CustomStats
{
    [XmlElement("CustomStat")]
    public List<CustomStat> Stats { get; set; }
}

public class CustomStat
{
    [XmlAttribute]
    public string mStatName { get; set; }

    [XmlAttribute]
    public string mValueForDisplay { get; set; }
}

public class MedalsCount
{
    [XmlElement("Medal")]
    public List<Medal> Medals { get; set; }
}

public class Medal
{
    [XmlAttribute]
    public int mId { get; set; }

    [XmlAttribute]
    public int mCount { get; set; }
}
