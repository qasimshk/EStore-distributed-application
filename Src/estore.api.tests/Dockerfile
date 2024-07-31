#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["estore.api.tests/estore.api.tests.csproj", "estore.api.tests/"]
COPY ["estore.api/estore.api.csproj", "estore.api/"]
COPY ["estore.common/estore.common.csproj", "estore.common/"]
RUN dotnet restore "./estore.api.tests/estore.api.tests.csproj"
COPY . .
WORKDIR "/src/estore.api.tests"
RUN dotnet build "./estore.api.tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./estore.api.tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "estore.api.tests.dll"]
