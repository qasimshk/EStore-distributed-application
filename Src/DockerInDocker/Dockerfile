FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /Src

COPY . .
WORKDIR /Src/estore.api

RUN dotnet restore "./estore.api/estore.api.csproj"

RUN dotnet build "./estore.api/estore.api.csproj" -c Release -o /app/build

RUN dotnet test "./estore.api.tests/estore.api.tests.csproj"

CMD dotnet test "./estore.api.tests/estore.api.tests.csproj"
