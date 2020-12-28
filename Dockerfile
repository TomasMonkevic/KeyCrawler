# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY KeyCrawler.WebApi/*.csproj ./KeyCrawler.WebApi/
COPY KeyCrawler.Service/*.csproj ./KeyCrawler.Service/
COPY KeyCrawler.Persistence/*.csproj ./KeyCrawler.Persistence/
COPY KeyCrawler.Domain/*.csproj ./KeyCrawler.Domain/
COPY KeyCrawler.Contracts/*.csproj ./KeyCrawler.Contracts/
RUN dotnet restore

# copy everything else and build app
COPY KeyCrawler.WebApi/. ./KeyCrawler.WebApi/
COPY KeyCrawler.Service/. ./KeyCrawler.Service/
COPY KeyCrawler.Persistence/. ./KeyCrawler.Persistence/
COPY KeyCrawler.Domain/. ./KeyCrawler.Domain/
COPY KeyCrawler.Contracts/. ./KeyCrawler.Contracts/
WORKDIR /source
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "KeyCrawler.WebApi.dll"]