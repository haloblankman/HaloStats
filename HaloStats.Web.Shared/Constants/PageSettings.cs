namespace HaloStats.Web.Shared.Constants;

public static class PageSettings
{
    public const int DefaultPlayerGamesPageSize = 100;
    

    public static class Leaderboard
    {
        public const int DefaultLeaderboardPlayersPageSize = 10;
        public const string SortKills = "kills";
        public const string SortKdr = "kdr";
        public const string DefaultSortBy = SortKills;
    }
}
