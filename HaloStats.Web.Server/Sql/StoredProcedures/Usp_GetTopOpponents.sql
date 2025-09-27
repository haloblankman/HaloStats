CREATE OR REPLACE FUNCTION Usp_GetTopOpponents(
    p_gamer_tag TEXT,
    p_game_enum INT DEFAULT NULL,
    p_limit INT DEFAULT 10
)
RETURNS TABLE (
    OpponentGamertag text,
    GamesPlayedAgainst integer,
    WinsAgainst integer,
    LossesAgainst integer,
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
    game_players AS (
        SELECT gp."GameId", gp."Gamertag", gp."TeamId", gp."IsWinner"
        FROM "GamePlayers" gp
        JOIN filtered_games fg ON gp."GameId" = fg."GameId"
    ),
    pairs AS (
        SELECT
            LEAST(gp1."Gamertag", gp2."Gamertag") AS player_one,
            GREATEST(gp1."Gamertag", gp2."Gamertag") AS player_two,
            gp1."GameId",
            gp1."TeamId" AS team_one,
            gp2."TeamId" AS team_two,
            gp1."IsWinner" AS player_one_won,
            gp2."IsWinner" AS player_two_won
        FROM game_players gp1
        JOIN game_players gp2
            ON gp1."GameId" = gp2."GameId"
            AND gp1."TeamId" <> gp2."TeamId"
            AND gp1."Gamertag" < gp2."Gamertag"
    ),
    pair_stats AS (
        SELECT
            player_one,
            player_two,
            COUNT(*) AS games_played,
            SUM(CASE WHEN player_one_won AND NOT player_two_won THEN 1 ELSE 0 END) AS wins,
            SUM(CASE WHEN NOT player_one_won AND player_two_won THEN 1 ELSE 0 END) AS losses
        FROM pairs
        WHERE (p_gamer_tag IS NULL OR player_one = p_gamer_tag OR player_two = p_gamer_tag)
        GROUP BY player_one, player_two
    )
    SELECT
        CASE
            WHEN pair_stats.player_one = p_gamer_tag THEN pair_stats.player_two
            ELSE pair_stats.player_one
        END AS "OpponentGamertag",
        pair_stats.games_played::integer AS "GamesPlayedAgainst",
        pair_stats.wins::integer AS "WinsAgainst",
        pair_stats.losses::integer AS "LossesAgainst",
        CASE WHEN pair_stats.games_played = 0 THEN 0
             ELSE pair_stats.wins::numeric / pair_stats.games_played END AS "WinRate"
    FROM pair_stats
    ORDER BY pair_stats.games_played DESC, pair_stats.wins DESC
    LIMIT p_limit;
END;
$$;