FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Web/TTcms.WebMVC/TTcms.WebMVC.csproj src/Web/TTcms.WebMVC/
COPY src/BuildingBlocks/Devspaces.Support/Devspaces.Support.csproj src/BuildingBlocks/Devspaces.Support/
RUN dotnet restore src/Web/TTcms.WebMVC/TTcms.WebMVC.csproj
COPY . .
WORKDIR /src/src/Web/TTcms.WebMVC
RUN dotnet build TTcms.WebMVC.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TTcms.WebMVC.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TTcms.WebMVC.dll"]
