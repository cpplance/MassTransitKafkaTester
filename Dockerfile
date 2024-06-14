FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MasstransitKafkaTester.csproj", "."]
RUN dotnet restore "./MasstransitKafkaTester.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MasstransitKafkaTester.csproj" -c Debug -o /app/build


FROM build AS publish
RUN dotnet publish "MasstransitKafkaTester.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_HTTP_PORTS=80
ENV DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP3SUPPORT=false

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MasstransitKafkaTester.dll"]
