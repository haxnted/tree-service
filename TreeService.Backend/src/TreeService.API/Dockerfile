FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TreeService.API/TreeService.API.csproj", "src/TreeService.API/"]
COPY ["src/TreeService.Infrastructure/TreeService.Infrastructure.csproj", "src/TreeService.Infrastructure/"]
COPY ["src/TreeService.Application/TreeService.Application.csproj", "src/TreeService.Application/"]
COPY ["src/TreeService.Domain/TreeService.Domain.csproj", "src/TreeService.Domain/"]
COPY ["src/TreeService.Contracts/TreeService.Contracts.csproj", "src/TreeService.Contracts/"]
RUN dotnet restore "src/TreeService.API/TreeService.API.csproj"
COPY . .
WORKDIR "/src/src/TreeService.API"
RUN dotnet build "TreeService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TreeService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TreeService.API.dll"]
