# Basis-Image für die Ausführung der Blazor-App (keine SSL-Konfiguration mehr erforderlich)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Wechsle nun zum nicht-Root-Benutzer
USER $APP_UID

# Build- und Publishing-Stufen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChoiceVFileSystemBlazor/ChoiceVFileSystemBlazor.csproj", "ChoiceVFileSystemBlazor/"]
COPY ["ChoiceVSharedApiModels/ChoiceVSharedApiModels.csproj", "ChoiceVSharedApiModels/"]
COPY ["ChoiceVRefitClient/ChoiceVRefitClient.csproj", "ChoiceVRefitClient/"]
RUN dotnet restore "ChoiceVFileSystemBlazor/ChoiceVFileSystemBlazor.csproj"
COPY . . 
WORKDIR "/src/ChoiceVFileSystemBlazor"
RUN dotnet build "ChoiceVFileSystemBlazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChoiceVFileSystemBlazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Finales Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Konfiguriere Kestrel so, dass es nur HTTP auf den Ports 8080 und 8081 akzeptiert
ENV ASPNETCORE_URLS="http://+:8080;http://+:8081"

ENTRYPOINT ["dotnet", "ChoiceVFileSystemBlazor.dll"]