# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY AspNET_Learn.sln .

# Copy all project files
COPY LibraryManagementAPI/LibraryManagementAPI.csproj LibraryManagementAPI/
COPY LibraryManagement.BusinessLogic/LibraryManagement.BusinessLogic.csproj LibraryManagement.BusinessLogic/
COPY LibraryManagement.DataAccess/LibraryManagement.DataAccess.csproj LibraryManagement.DataAccess/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build
WORKDIR /src/LibraryManagementAPI
RUN dotnet build -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && \
    chown -R appuser:appuser /app

USER appuser

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "LibraryManagementAPI.dll"]
