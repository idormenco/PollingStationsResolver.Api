# RUN ALL CONTAINERS FROM ROOT (folder with .sln file):
# docker-compose build
# docker-compose up
#
# RUN JUST THIS CONTAINER FROM ROOT (folder with .sln file):
# docker build --pull -t resolver-api -f src/PollingStationsResolver.Api/Dockerfile .
#
# RUN COMMAND
#  docker run --name resolver-api --rm -it resolver-api

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY ["src/PollingStationsResolver.Api/PollingStationsResolver.Api.csproj", "PollingStationsResolver.Api/"]
COPY ["src/PollingStationsResolver.Geocoding/PollingStationsResolver.Geocoding.csproj", "PollingStationsResolver.Geocoding/"]
COPY ["src/PollingStationsResolver.Domain/PollingStationsResolver.Domain.csproj", "PollingStationsResolver.Domain/"]

RUN dotnet restore "PollingStationsResolver.Api/PollingStationsResolver.Api.csproj"

COPY ["src/PollingStationsResolver.Api", "PollingStationsResolver.Api/"]
COPY ["src/PollingStationsResolver.Geocoding", "PollingStationsResolver.Geocoding/"]
COPY ["src/PollingStationsResolver.Domain", "PollingStationsResolver.Domain/"]

WORKDIR /app/PollingStationsResolver.Api
RUN ls
RUN dotnet publish "PollingStationsResolver.Api.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/PollingStationsResolver.Api/out ./

# Optional: Set this here if not setting it from docker-compose.yml
# ENV ASPNETCORE_ENVIRONMENT Development

ENTRYPOINT ["dotnet", "PollingStationsResolver.Api.dll"]
