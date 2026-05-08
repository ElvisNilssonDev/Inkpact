# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first for layer caching
COPY src/Domain/*.csproj src/Domain/
COPY src/Application/*.csproj src/Application/
COPY src/Infrastructure/*.csproj src/Infrastructure/
COPY src/InkpactAPI/*.csproj src/InkpactAPI/

# Restore dependencies
RUN dotnet restore src/InkpactAPI/InkpactAPI.csproj

# Copy source code and publish
COPY src/ src/
RUN dotnet publish src/InkpactAPI/InkpactAPI.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage (smaller image, no SDK)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "InkpactAPI.dll"]
