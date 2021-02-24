FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Catalog/TTcms.Catalog.API/TTcms.Catalog.API.csproj src/Services/Catalog/TTcms.Catalog.API/
COPY src/BuildingBlocks/EventBus/TTcms.IntegrationEventLogEF/TTcms.IntegrationEventLogEF.csproj src/BuildingBlocks/EventBus/TTcms.IntegrationEventLogEF/
COPY src/BuildingBlocks/EventBus/TTcms.EventBus/TTcms.EventBus.csproj src/BuildingBlocks/EventBus/TTcms.EventBus/
COPY src/BuildingBlocks/EventBus/TTcms.EventBusServiceBus/TTcms.EventBusServiceBus.csproj src/BuildingBlocks/EventBus/TTcms.EventBusServiceBus/
COPY src/BuildingBlocks/WebHost/WebHost.Customization/WebHost.Customization.csproj src/BuildingBlocks/WebHost/WebHost.Customization/
COPY src/BuildingBlocks/EventBus/TTcms.EventBusRabbitMQ/TTcms.EventBusRabbitMQ.csproj src/BuildingBlocks/EventBus/TTcms.EventBusRabbitMQ/
RUN dotnet restore src/Services/Catalog/TTcms.Catalog.API/TTcms.Catalog.API.csproj
COPY . .
WORKDIR /src/src/Services/Catalog/TTcms.Catalog.API
RUN dotnet build TTcms.Catalog.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TTcms.Catalog.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TTcms.Catalog.API.dll"]
