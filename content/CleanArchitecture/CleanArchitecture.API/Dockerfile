# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CleanArchitecture.API/CleanArchitecture.API.csproj", "CleanArchitecture.API/"]
COPY ["CleanArchitecture.Application/CleanArchitecture.Application.csproj", "CleanArchitecture.Application/"]
COPY ["CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "CleanArchitecture.Domain/"]
COPY ["CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "CleanArchitecture.Infrastructure/"]
COPY ["CleanArchitecture.Persistence/CleanArchitecture.Persistence.csproj", "CleanArchitecture.Persistence/"]
COPY ["CleanArchitecture.Migrations/CleanArchitecture.Migrations.csproj", "CleanArchitecture.Migrations/"]
RUN --mount=type=cache,id=nuget-api,target=/root/.nuget/packages \
    dotnet restore "./CleanArchitecture.API/CleanArchitecture.API.csproj"
COPY . .
WORKDIR "/src/CleanArchitecture.API"
RUN --mount=type=cache,id=nuget-api,target=/root/.nuget/packages \
    dotnet build "./CleanArchitecture.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,id=nuget-api,target=/root/.nuget/packages \
    dotnet publish "./CleanArchitecture.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.API.dll"]
