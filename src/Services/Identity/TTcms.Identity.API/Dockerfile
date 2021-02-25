FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR ./src
COPY ../src/Services/Identity/TTcms.Identity.API/TTcms.Identity.API.csproj src/Services/Identity/TTcms.Identity.API/
RUN dotnet restore ./src/Services/Identity/TTcms.Identity.API/TTcms.Identity.API.csproj
COPY . .
WORKDIR ./src/src/Services/Identity/TTcms.Identity.API
RUN dotnet build TTcms.Identity.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TTcms.Identity.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TTcms.Identity.API.dll"]
