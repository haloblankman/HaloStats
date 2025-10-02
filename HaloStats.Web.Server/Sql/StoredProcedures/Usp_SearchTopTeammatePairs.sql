CREATE OR REPLACE FUNCTION Usp_SearchTopTeammatePairs(
    p_game_enum SMALLINT,
    p_is_monthly BOOLEAN,
    p_is_matchmaking BOOLEAN,
    p_is_customs BOOLEAN,
    p_search_gamertag TEXT,
    p_minimum_amount_of_games_played_together INT,
    p_sorted_by TEXT, -- WinsTogether, GamesPlayedTogether, WinRate
    p_sort_direction TEXT, -- 'asc' or 'desc'
    p_page_number INT,
    p_page_size INT
)
RETURNS TABLE (
    "Rank" INT,
    PlayerOneGamertag text,
    PlayerTwoGamertag text,
    GamesPlayedTogether integer,
    WinsTogether integer,
    LossesTogether integer,
    WinRate numeric,
    TotalCount int
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_one_month_ago TIMESTAMP := NOW() - INTERVAL '1 month';
    v_offset INT := GREATEST((p_page_number - 1) * p_page_size, 0);
    v_order_by TEXT;
    v_total_count INT;
BEGIN

    -- Main CTE: aggregate player stats
    RETURN QUERY
    WITH filtered_games AS (
        SELECT g."GameId"
        FROM "Games" g
        WHERE g."IsTeamsEnabled" = TRUE
          AND g."IsDuplicateGame" = FALSE
          AND g."IsDeleted" = FALSE
          AND (p_game_enum IS NULL OR g."GameEnum" = p_game_enum)
          AND (NOT p_is_monthly OR g."ReportedAt" >= v_one_month_ago)
          AND (
                (p_is_matchmaking = FALSE AND p_is_customs = FALSE)
                OR (p_is_matchmaking = TRUE AND g."IsMatchmaking" = TRUE)
                OR (p_is_customs = TRUE AND g."IsMatchmaking" = FALSE)
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
            SUM(CASE WHEN NOT both_won THEN 1 ELSE 0 END) AS losses,
            CASE WHEN COUNT(*) = 0 THEN 0
                 ELSE SUM(CASE WHEN both_won THEN 1 ELSE 0 END)::numeric / COUNT(*) END AS win_rate
        FROM pairs
        GROUP BY player_one, player_two
        HAVING COUNT(*) >= p_minimum_amount_of_games_played_together
    ),
    ranked_pairs AS (
        SELECT
            ROW_NUMBER() OVER (
                ORDER BY
                    CASE WHEN p_sorted_by = 'GamesPlayedTogether' THEN games_played END DESC,
                    CASE WHEN p_sorted_by = 'WinsTogether' THEN wins END DESC,
                    CASE WHEN p_sorted_by = 'WinRate' THEN win_rate END DESC,
					CASE WHEN p_sorted_by = 'LossesTogether' THEN win_rate END DESC,
                    win_rate DESC, wins DESC, player_one ASC
            ) AS rank,
            player_one,
            player_two,
            games_played,
            wins,
            losses,
            win_rate
        FROM pair_stats
    ),
    filtered_pairs AS (
        SELECT *
        FROM ranked_pairs
        WHERE (p_search_gamertag IS NULL OR player_one = p_search_gamertag OR player_two = p_search_gamertag)
    )
    SELECT
        rank::int AS "Rank",
        CASE
            WHEN p_search_gamertag IS NULL THEN player_one
            WHEN player_one = p_search_gamertag THEN player_one
            ELSE player_two
        END AS "PlayerOneGamertag",
        CASE
            WHEN p_search_gamertag IS NULL THEN player_two
            WHEN player_one = p_search_gamertag THEN player_two
            ELSE player_one
        END AS "PlayerTwoGamertag",
        games_played::integer AS "GamesPlayedTogether",
        wins::integer AS "WinsTogether",
        losses::integer AS "LossesTogether",
        win_rate::numeric AS "WinRate",
        (SELECT COUNT(*) FROM filtered_pairs)::INT as "TotalCount"
    FROM filtered_pairs
    ORDER BY
		CASE WHEN LOWER(p_sort_direction) = 'desc' THEN rank END ASC,
		CASE WHEN LOWER(p_sort_direction) = 'asc' THEN rank END DESC
    OFFSET v_offset
    LIMIT p_page_size + 1;
END;
$$;
