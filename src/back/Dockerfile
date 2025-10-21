FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY triincom/triincom.csproj triincom/
COPY triincom.Application/triincom.Application.csproj triincom.Application/
COPY triincom.DataPersistence/triincom.DataPersistence.csproj triincom.DataPersistence/
COPY triincom.Core/triincom.Core.csproj triincom.Core/
COPY triincom.Infrastructure/triincom.Infrastructure.csproj triincom.Infrastructure/
COPY triincom.Tests/triincom.Tests.csproj triincom.Tests/

RUN dotnet restore "triincom/triincom.csproj"
RUN dotnet restore "triincom.Core/triincom.Core.csproj"
RUN dotnet restore "triincom.Application/triincom.Application.csproj"
RUN dotnet restore "triincom.DataPersistence/triincom.DataPersistence.csproj"
RUN dotnet restore "triincom.Infrastructure/triincom.Infrastructure.csproj"
RUN dotnet restore "triincom.Tests/triincom.Tests.csproj"

COPY . .

WORKDIR /src
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

WORKDIR /src/triincom.Tests
RUN dotnet test "triincom.Tests.csproj" -c $BUILD_CONFIGURATION --verbosity normal

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/triincom
RUN dotnet publish "triincom.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "triincom.dll"]