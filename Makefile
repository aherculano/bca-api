build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
run:
	dotnet run --project src/Api/Api.csproj
test:
	dotnet test tests/UnitTests
build-docker:
	docker build -t bca-api:latest .
run-docker:
	docker run -p 5000:8080 bca-api:latest
run-migrations:
	dotnet ef database update --project src/Migrations/Migrations.csproj
run-environment:
	docker-compose up -d --build