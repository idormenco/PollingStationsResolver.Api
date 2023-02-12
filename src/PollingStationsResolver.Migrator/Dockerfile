# RUN ALL CONTAINERS FROM ROOT (folder with .sln file):
# docker-compose build
# docker-compose up
#
# RUN JUST THIS CONTAINER FROM ROOT (folder with .sln file):
# docker build --pull -t migrator -f src/PollingStationsResolver.Migrator/Dockerfile .
#
# RUN COMMAND
#  docker run --name migrator --rm -it migrator

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY ["src/PollingStationsResolver.Migrator/PollingStationsResolver.Migrator.csproj", "PollingStationsResolver.Migrator/"]
COPY ["src/PollingStationsResolver.Domain/PollingStationsResolver.Domain.csproj", "PollingStationsResolver.Domain/"]

RUN dotnet restore "PollingStationsResolver.Migrator/PollingStationsResolver.Migrator.csproj"
COPY ["src/PollingStationsResolver.Migrator", "PollingStationsResolver.Migrator/"]
COPY ["src/PollingStationsResolver.Domain", "PollingStationsResolver.Domain/"]

WORKDIR /app/PollingStationsResolver.Migrator
RUN dotnet publish "PollingStationsResolver.Migrator.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/PollingStationsResolver.Migrator/out ./

# Optional: Set this here if not setting it from docker-compose.yml
# ENV ASPNETCORE_ENVIRONMENT Development

ENTRYPOINT ["dotnet", "PollingStationsResolver.Migrator.dll"]
