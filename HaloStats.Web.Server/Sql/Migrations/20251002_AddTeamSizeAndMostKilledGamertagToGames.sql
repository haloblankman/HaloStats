UPDATE "Games" g
SET "TeamSize" = teamsize
FROM (
    select gs."GameId", max(sub.count) teamsize
    FROM "Games" gs, 
    (
        SELECT "GameId", "TeamId", COUNT(*) as count
        FROM "GamePlayers"
        GROUP BY "GameId", "TeamId"
    ) as sub
    WHERE gs."GameId" = sub."GameId"
    AND gs."IsTeamsEnabled" = true
    GROUP BY gs."GameId"
) as sub
WHERE g."GameId" = sub."GameId"
AND g."IsTeamsEnabled" = true;

-- Fill in KilledMostGamertag based on KilledMostPlayerIndex
UPDATE "GamePlayers" gp
SET "KilledMostGamertag" = sub."Gamertag"
FROM (
    SELECT
        gp1."GamePlayerId",
        CASE
            WHEN gp1."KilledMostPlayerIndex" = -1 THEN NULL
            ELSE gp2."Gamertag"
        END AS "Gamertag"
    FROM "GamePlayers" gp1
    LEFT JOIN (
        SELECT
            gp_inner."GameId",
            gp_inner."Gamertag",
            ROW_NUMBER() OVER (PARTITION BY gp_inner."GameId" ORDER BY gp_inner."GamePlayerId") - 1 AS idx
        FROM "GamePlayers" gp_inner
    ) gp2
    ON gp1."GameId" = gp2."GameId"
    AND gp1."KilledMostPlayerIndex" = gp2.idx
) sub
WHERE gp."GamePlayerId" = sub."GamePlayerId";

-- Fill in MostKilledByGamertag based on MostKilledByPlayerIndex
UPDATE "GamePlayers" gp
SET "MostKilledByGamertag" = sub."Gamertag"
FROM (
    SELECT
        gp1."GamePlayerId",
        CASE
            WHEN gp1."MostKilledByPlayerIndex" = -1 THEN NULL
            ELSE gp2."Gamertag"
        END AS "Gamertag"
    FROM "GamePlayers" gp1
    LEFT JOIN (
        SELECT
            gp_inner."GameId",
            gp_inner."Gamertag",
            ROW_NUMBER() OVER (PARTITION BY gp_inner."GameId" ORDER BY gp_inner."GamePlayerId") - 1 AS idx
        FROM "GamePlayers" gp_inner
    ) gp2
    ON gp1."GameId" = gp2."GameId"
    AND gp1."MostKilledByPlayerIndex" = gp2.idx
) sub
WHERE gp."GamePlayerId" = sub."GamePlayerId";
