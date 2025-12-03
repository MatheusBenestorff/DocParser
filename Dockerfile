# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/DocParser.Core/DocParser.Core.csproj", "DocParser.Core/"]
COPY ["src/DocParser.CLI/DocParser.CLI.csproj", "DocParser.CLI/"]
RUN dotnet restore "DocParser.CLI/DocParser.CLI.csproj"

COPY src/ .
WORKDIR /src/DocParser.CLI
RUN dotnet publish -c Release -o /app/publish

#Run
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

VOLUME /data

ENTRYPOINT ["dotnet", "DocParser.CLI.dll"]