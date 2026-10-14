FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY JobTrack.Api/JobTrack.Api.csproj JobTrack.Api/
RUN dotnet restore JobTrack.Api/JobTrack.Api.csproj

COPY . .
RUN dotnet publish JobTrack.Api/JobTrack.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "JobTrack.Api.dll"]