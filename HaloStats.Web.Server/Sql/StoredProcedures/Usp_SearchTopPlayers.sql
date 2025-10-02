CREATE OR REPLACE FUNCTION Usp_SearchTopPlayers(
    p_game_enum SMALLINT,
    p_is_monthly BOOLEAN,
    p_search_gamertag TEXT,
    p_sorted_by TEXT,
    p_sort_direction TEXT, -- 'asc' or 'desc'
    p_page_number INT,
    p_page_size INT
)
RETURNS TABLE (
    "Rank" INT,
    "Gamertag" TEXT,
    "Kills" INT,
    "Deaths" INT,
    "KillDeathRatio" NUMERIC,
    "TotalCount" INT
) AS $$
DECLARE
    v_one_month_ago TIMESTAMP := NOW() - INTERVAL '1 month';
    v_offset INT := GREATEST((p_page_number - 1) * p_page_size, 0);
    v_order_by TEXT;
    v_total_count INT;
BEGIN
    -- Determine order by clause
    IF p_sorted_by IS NULL OR LOWER(p_sorted_by) = 'kills' THEN
        v_order_by := 'kills';
    ELSE
        v_order_by := 'kill_death_ratio';
    END IF;

    -- Main CTE: aggregate player stats
    RETURN QUERY
    WITH filtered_games AS (
        SELECT g."GameId"
        FROM "Games" g
        WHERE g."IsDuplicateGame" = FALSE
          AND g."IsDeleted" = FALSE
          AND (p_game_enum IS NULL OR g."GameEnum" = p_game_enum)
          AND (NOT p_is_monthly OR g."ReportedAt" >= v_one_month_ago)
    ),
    player_stats AS (
        SELECT
            gp."Gamertag" as gamertag,
            SUM(gp."Kills") AS kills,
            SUM(gp."Deaths") AS deaths,
            CASE WHEN SUM(gp."Deaths") = 0 THEN SUM(gp."Kills")
                 ELSE SUM(gp."Kills")::NUMERIC / NULLIF(SUM(gp."Deaths"), 0)
            END AS kill_death_ratio
        FROM "GamePlayers" gp
        JOIN filtered_games g ON gp."GameId" = g."GameId"
        GROUP BY gp."Gamertag"
        HAVING SUM(gp."Kills") >= 100
    ),
    ranked_players AS (
        SELECT
            *,
            ROW_NUMBER() OVER (
                ORDER BY
                    CASE WHEN v_order_by = 'kills' THEN kills END DESC,
                    CASE WHEN v_order_by = 'kill_death_ratio' THEN kill_death_ratio END DESC,
                    gamertag ASC
            ) AS rank
        FROM player_stats
    ),
    filtered_players AS (
        SELECT *
        FROM ranked_players
        WHERE (p_search_gamertag IS NULL OR gamertag = p_search_gamertag)
    )
    SELECT
        rank::INT as "Rank",
        gamertag as "GamerTag",
        kills::INT as "Kills",
        deaths::INT as "Deaths",
        kill_death_ratio as "KillDeathRatio",
        (SELECT COUNT(*) FROM filtered_players)::INT as "TotalCount"
    FROM filtered_players
    ORDER BY
	    CASE WHEN v_order_by = 'kills' AND LOWER(p_sort_direction) = 'desc' THEN kills END DESC,
	    CASE WHEN v_order_by = 'kills' AND LOWER(p_sort_direction) = 'asc' THEN kills END ASC,
	    CASE WHEN v_order_by = 'kill_death_ratio' AND LOWER(p_sort_direction) = 'desc' THEN kill_death_ratio END DESC,
	    CASE WHEN v_order_by = 'kill_death_ratio' AND LOWER(p_sort_direction) = 'asc' THEN kill_death_ratio END ASC
    OFFSET CASE WHEN p_search_gamertag IS NULL THEN v_offset ELSE 0 END
    LIMIT CASE WHEN p_search_gamertag IS NULL THEN p_page_size + 1 ELSE 1 END;
END;
$$ LANGUAGE plpgsql;