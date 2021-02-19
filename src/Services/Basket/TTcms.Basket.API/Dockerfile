FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Basket/TTcms.Basket.API/TTcms.Basket.API.csproj src/Services/Basket/TTcms.Basket.API/
COPY src/BuildingBlocks/EventBus/TTcms.EventBusServiceBus/TTcms.EventBusServiceBus.csproj src/BuildingBlocks/EventBus/TTcms.EventBusServiceBus/
COPY src/BuildingBlocks/EventBus/TTcms.EventBus/TTcms.EventBus.csproj src/BuildingBlocks/EventBus/TTcms.EventBus/
COPY src/BuildingBlocks/EventBus/TTcms.EventBusRabbitMQ/TTcms.EventBusRabbitMQ.csproj src/BuildingBlocks/EventBus/TTcms.EventBusRabbitMQ/
RUN dotnet restore src/Services/Basket/TTcms.Basket.API/TTcms.Basket.API.csproj
COPY . .
WORKDIR /src/src/Services/Basket/TTcms.Basket.API
RUN dotnet build TTcms.Basket.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TTcms.Basket.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TTcms.Basket.API.dll"]
