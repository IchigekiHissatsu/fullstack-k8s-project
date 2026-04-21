FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# projekt fájl másolása és csomagok letöltése
COPY *.csproj ./
RUN dotnet restore

# maradék másolása és publikálása
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime kép készítése
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Port beállítása (a konténeren belül a 80-as porton)
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "InventoryService.dll"]