FROM alpine:3.17 AS base
RUN apk update \
    && apk upgrade \
    && apk add git \
    && apk add dotnet6-runtime
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
RUN git clone "https://github.com/dimon4egkl/test_git_library"
ENTRYPOINT ["dotnet", "TestConsoleProject.dll"]
