CREATE OR REPLACE FUNCTION Usp_GetTopTeammatePairs(
    p_gamer_tag TEXT DEFAULT NULL,
    p_game_enum INT DEFAULT NULL,
    p_limit INT DEFAULT 10
)
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
                p_game_enum IS NULL
                OR g."GameEnum" = p_game_enum
              )
    ),
    team_players AS (
        SELECT gp."GameId", gp."Gamertag", gp."TeamId", gp."IsWinner"
        FROM "GamePlayers" gp
        JOIN filtered_games fg ON gp."GameId" = fg."GameId"
    ),
    pairs AS (
        SELECT
            LEAST(tp1."Gamertag", tp2."Gamertag") AS player_one,
            GREATEST(tp1."Gamertag", tp2."Gamertag") AS player_two,
            tp1."GameId",
            tp1."TeamId",
            (tp1."IsWinner" AND tp2."IsWinner") AS both_won
        FROM team_players tp1
        JOIN team_players tp2
            ON tp1."GameId" = tp2."GameId"
            AND tp1."TeamId" = tp2."TeamId"
            AND tp1."Gamertag" < tp2."Gamertag"
    ),
    pair_stats AS (
        SELECT
            player_one,
            player_two,
            COUNT(*) AS games_played,
            SUM(CASE WHEN both_won THEN 1 ELSE 0 END) AS wins,
            SUM(CASE WHEN NOT both_won THEN 1 ELSE 0 END) AS losses
        FROM pairs
        where (p_gamer_tag IS NULL OR player_one = p_gamer_tag OR player_two = p_gamer_tag)
        GROUP BY player_one, player_two
    )
    SELECT
        CASE
            WHEN p_gamer_tag IS NULL THEN pair_stats.player_one
            WHEN pair_stats.player_one = p_gamer_tag THEN pair_stats.player_one
            ELSE pair_stats.player_two
        END AS "PlayerOneGamertag",
        CASE
            WHEN p_gamer_tag IS NULL THEN pair_stats.player_two
            WHEN pair_stats.player_one = p_gamer_tag THEN pair_stats.player_two
            ELSE pair_stats.player_one
        END AS "PlayerTwoGamertag",
        pair_stats.games_played::integer AS "GamesPlayedTogether",
        pair_stats.wins::integer AS "WinsTogether",
        pair_stats.losses::integer AS "LossesTogether",
        CASE WHEN pair_stats.games_played = 0 THEN 0
             ELSE pair_stats.wins::numeric / pair_stats.games_played END AS "WinRate"
    FROM pair_stats
    ORDER BY pair_stats.games_played DESC, pair_stats.wins DESC
    LIMIT p_limit;
END;
$$;