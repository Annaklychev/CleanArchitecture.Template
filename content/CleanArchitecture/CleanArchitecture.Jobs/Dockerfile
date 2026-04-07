# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CleanArchitecture.Jobs/CleanArchitecture.Jobs.csproj", "CleanArchitecture.Jobs/"]
COPY ["CleanArchitecture.Application/CleanArchitecture.Application.csproj", "CleanArchitecture.Application/"]
COPY ["CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "CleanArchitecture.Domain/"]
RUN --mount=type=cache,id=nuget-jobs,target=/root/.nuget/packages \
    dotnet restore "./CleanArchitecture.Jobs/CleanArchitecture.Jobs.csproj"
COPY . .
WORKDIR "/src/CleanArchitecture.Jobs"
RUN --mount=type=cache,id=nuget-jobs,target=/root/.nuget/packages \
    dotnet build "./CleanArchitecture.Jobs.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,id=nuget-jobs,target=/root/.nuget/packages \
    dotnet publish "./CleanArchitecture.Jobs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Jobs.dll"]
