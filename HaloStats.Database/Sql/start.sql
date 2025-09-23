
-- Connect to postgres using superuser (eg. postgres) and run the following commands
-- Create the database
CREATE DATABASE halostatsdb;

-- Create the deploy user (for migrations)
CREATE USER halostatsdeploy WITH PASSWORD 'halo_stats_deploy';

-- Create the app user (for runtime)
CREATE USER halostatsapp WITH PASSWORD 'halo_stat_app';

-- Grant all privileges to deploy user (for migrations)
GRANT ALL PRIVILEGES ON DATABASE halostatsdb TO halostatsdeploy;

-- Grant connect and usage to app user
GRANT CONNECT ON DATABASE halostatsdb TO halostatsapp;

GRANT USAGE ON SCHEMA public TO halostatsdeploy;
GRANT CREATE ON SCHEMA public TO halostatsdeploy;







-- now connect to the halostatdb using user halostatsdeploy
\c halostatsdb

-- Grant usage on schema to app user
GRANT USAGE ON SCHEMA public TO halostatsapp;

-- Grant CRUD permissions to app user (for all tables, sequences, functions)
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO halostatsapp;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO halostatsapp;
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public TO halostatsapp;

-- Ensure future tables/sequences/functions get the same privileges
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO halostatsapp;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE, SELECT ON SEQUENCES TO halostatsapp;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT EXECUTE ON FUNCTIONS TO halostatsapp;