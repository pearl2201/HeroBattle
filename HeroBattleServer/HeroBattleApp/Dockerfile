# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HeroBattleServer/HeroBattleApp/HeroBattleApp.csproj", "HeroBattleServer/HeroBattleApp/"]
COPY ["HeroBattleUnity/Assets/Plugins/Dependencies", "HeroBattleUnity/Assets/Plugins/Dependencies"]
COPY ["HeroBattleUnity/Assets/Core/Scripts/HeroBattleServer/HeroBattleServer.csproj", "HeroBattleUnity/Assets/Core/Scripts/HeroBattleServer/"]
COPY ["HeroBattleUnity/Assets/Plugins/LiteEntitySystem/LiteEntitySystem.csproj", "HeroBattleUnity/Assets/Plugins/LiteEntitySystem/"]
COPY ["HeroBattleUnity/Assets/Plugins/LiteNetLib/LiteNetLib.csproj", "HeroBattleUnity/Assets/Plugins/LiteNetLib/"]
COPY ["HeroBattleUnity/Assets/Core/Scripts/HeroBattleShare/HeroBattleShare.csproj", "HeroBattleUnity/Assets/Core/Scripts/HeroBattleShare/"]
RUN dotnet restore "./HeroBattleServer/HeroBattleApp/HeroBattleApp.csproj"
COPY . .
WORKDIR "/src/HeroBattleServer/HeroBattleApp"
RUN dotnet build "./HeroBattleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HeroBattleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeroBattleApp.dll"]
