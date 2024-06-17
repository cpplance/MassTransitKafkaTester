FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["MtKafkaIssueConsumer/MtKafkaIssueConsumer.csproj", "MtKafkaIssueConsumer/"]
RUN dotnet restore "MtKafkaIssueConsumer/MtKafkaIssueConsumer.csproj"
COPY . .
WORKDIR "/src/MtKafkaIssueConsumer"
RUN dotnet build "MtKafkaIssueConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "MtKafkaIssueConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MtKafkaIssueConsumer.dll"]
