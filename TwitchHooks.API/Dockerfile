FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TwitchHooks.API/TwitchHooks.API.csproj", "TwitchHooks.API/"]
RUN dotnet restore "TwitchHooksAPI/TwitchHooksAPI.csproj"
COPY . .
WORKDIR "/src/TwitchHooks.API"
RUN dotnet build "TwitchHooks.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwitchHooks.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwitchHooks.API.dll"]
