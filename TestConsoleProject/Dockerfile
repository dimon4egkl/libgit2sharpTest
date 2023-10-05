FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestConsoleProject/TestConsoleProject.csproj", "TestConsoleProject/"]
COPY ["LibGit2Sharp/LibGit2Sharp.csproj", "LibGit2Sharp/"]
RUN dotnet restore "TestConsoleProject/TestConsoleProject.csproj"
COPY . .
WORKDIR "/src/TestConsoleProject"
RUN dotnet build "TestConsoleProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestConsoleProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestConsoleProject.dll"]
