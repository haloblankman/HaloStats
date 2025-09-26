-- HaloStats.Web.Server/Database/StoredProcedures/GetTopTeammatePairs.sql
CREATE OR REPLACE FUNCTION Usp_GetTopTeammatePairs(game_enum integer)
RETURNS TABLE (
    PlayerOneGamertag text,
    PlayerTwoGamertag text,
    GamesPlayedTogether integer,
    WinsTogether integer,
    LossesTogether integer,
    WinRate numeric
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    WITH filtered_games AS (
        SELECT g."GameId"
        FROM "Games" g
        WHERE g."IsTeamsEnabled" = TRUE
          AND g."IsDuplicateGame" = FALSE
          AND g."IsDeleted" = FALSE
          AND (
                game_enum = -1
                OR g."GameEnum" = game_enum
              )
    ),
    team_players AS (
        SELECT gp."GameId", gp."GamerTag", gp."TeamId", gp."IsWinner"
        FROM "GamePlayers" gp
        JOIN filtered_games fg ON gp."GameId" = fg."GameId"
    ),
    pairs AS (
        SELECT
            LEAST(tp1."GamerTag", tp2."GamerTag") AS PlayerOneGamertag,
            GREATEST(tp1."GamerTag", tp2."GamerTag") AS PlayerTwoGamertag,
            tp1."GameId",
            tp1."TeamId",
            (tp1."IsWinner" AND tp2."IsWinner") AS BothWon
        FROM team_players tp1
        JOIN team_players tp2
            ON tp1."GameId" = tp2."GameId"
            AND tp1."TeamId" = tp2."TeamId"
            AND tp1."GamerTag" < tp2."GamerTag"
    ),
    pair_stats AS (
        SELECT
            PlayerOneGamertag,
            PlayerTwoGamertag,
            COUNT(*) AS GamesPlayedTogether,
            SUM(CASE WHEN BothWon THEN 1 ELSE 0 END) AS WinsTogether,
            SUM(CASE WHEN NOT BothWon THEN 1 ELSE 0 END) AS LossesTogether
        FROM pairs
        GROUP BY player_one, player_two
    )
    SELECT
        PlayerOneGamertag,
        PlayerTwoGamertag,
        GamesPlayedTogether,
        WinsTogether,
        LossesTogether,
        CASE WHEN GamesPlayedTogether = 0 THEN 0
             ELSE WinsTogether::numeric / GamesPlayedTogether END AS WinRate
    FROM pair_stats
    ORDER BY GamesPlayedTogether DESC, WinsTogether DESC
    LIMIT 10;
END;
$$;