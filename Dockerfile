FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
WORKDIR /src
COPY ["ACS.Core/ACS.Core.csproj", "ACS.Core/"]
COPY ["ACS.Infrastructure/ACS.Infrastructure.csproj", "ACS.Infrastructure/"]
COPY ["ACS.API/ACS.API.csproj", "ACS.API/"]

RUN dotnet restore "ACS.API/ACS.API.csproj"

# Copy everything else and build
COPY . ./
WORKDIR "/src/ACS.API"
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ACS.API.dll"]
