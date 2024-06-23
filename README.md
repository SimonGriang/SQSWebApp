Dies ist das SQS-Projekt von Simon Göttsberger für die Vorlesung

Software Qualitätssicherung


Durchgeführte Tests:

- Integration Test (somehow LanguageRepositoryIntegrationTest/TranslationRepositoryIntegrationTest and E2E Tests in the same Pipeline lead to conflicts, that's why those two test classes are commented)
- Unit Test
- E2E Test
- Dependency Security Checks (Github Action)






Personell Notes:

ConnectionString für MS LocalDB:
    "WebAppContext": "Server=(localdb)\\mssqllocaldb;Database=WebAppContext-aa547cdf-dffb-4433-af90-ecd6f6d5ac75;Trusted_Connection=True;MultipleActiveResultSets=true"

Run Docker Postgres Image
    docker run --name postgresDB -e POSTGRES_PASSWORD=mypassword -p 5432:5432 -d postgres

Create Migration
    dotnet ef migrations add MigrationName

Update Database
    dotnet ef Update-Database

Anwendung Starten
    dotnet run

Set up SonarCloud Pipeline
