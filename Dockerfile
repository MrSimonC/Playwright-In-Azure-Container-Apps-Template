FROM mcr.microsoft.com/playwright/dotnet:v1.27.0-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PlaywrightDemo.csproj", "./"]
RUN dotnet restore "PlaywrightDemo.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PlaywrightDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlaywrightDemo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# mcr.microsoft.com/playwright/dotnet doesn't include edge.
# Install Edge, matching the same container install locations:
ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright/
RUN pwsh /app/publish/playwright.ps1 install msedge
RUN ls /ms-playwright/
# ENTRYPOINT ["dotnet", "PlaywrightDemo.dll"]
