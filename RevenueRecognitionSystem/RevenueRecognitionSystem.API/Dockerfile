# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY *.sln ./
COPY RevenueRecognitionSystem.API/*.csproj ./RevenueRecognitionSystem.API/
COPY RevenueRecognitionSystem.Services/*.csproj ./RevenueRecognitionSystem.Services/
COPY RevenueRecognitionSystem.Domain/*.csproj ./RevenueRecognitionSystem.Domain/
COPY RevenueRecognitionSystem.Infrastructure/*.csproj ./RevenueRecognitionSystem.Infrastructure/


RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish RevenueRecognitionSystem.API/RevenueRecognitionSystem.API.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "RevenueRecognitionSystem.API.dll"]