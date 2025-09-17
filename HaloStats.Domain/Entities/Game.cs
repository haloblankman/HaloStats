namespace HaloStats.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public Guid GameUniqueId { get; set; }
        public bool LastMatchIncomplete { get; set; }
        public bool IsTeamsEnabled { get; set; }
        public string GameTypeName { get; set; }
        public List<PlayerScore> PlayerScores { get; set; }
    }

    public class  PlayerScore
    {
        public Guid GameId { get; set; }
        public int PlayerIndex { get; set; }
        public string XboxUserId { get; set; }
        public bool IsGuest { get; set; }
        public string GamerTag { get; set; }
        public string ClanTag { get; set; }
        public int TeamId { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int Score { get; set; }
        public int Betrayls { get; set; }
        public int MostKillsInARow { get; set; }
        public int Suicides { get; set; }
        public int SecondsAlive { get; set; }
        public int SecondsPlayed { get; set; }
        public int KilledMostPlayerIndex { get; set; }
        public int KilledMostPlayerCount { get; set; }
        public int MostKilledByPlayerIndex { get; set; }
        public int MostKilledByPlayerCount { get; set; }
        public int MostUsedWeapon { get; set; }
        public int MostUsedWeaponCount { get; set; }
        public List<string> Medals { get; set; }
    }
}
