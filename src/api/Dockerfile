FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/api/MediaArtArchive.Api.csproj", "src/api/"]
COPY ["src/MediaArtArchive.ServiceDefaults/MediaArtArchive.ServiceDefaults.csproj", "src/MediaArtArchive.ServiceDefaults/"]
RUN dotnet restore "src/api/MediaArtArchive.Api.csproj"

COPY . .
RUN dotnet publish "src/api/MediaArtArchive.Api.csproj" --configuration Release --no-restore --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
USER $APP_UID
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MediaArtArchive.Api.dll"]
