FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /Src

COPY . .

RUN dotnet restore

CMD dotnet test "./estore.api.tests/estore.api.tests.csproj"
