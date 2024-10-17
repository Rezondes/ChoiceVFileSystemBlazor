FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Kopiere das Zertifikat als Root
COPY blazorapp.pfx /https/blazorapp.pfx
# Ändere den Besitzer der Zertifikatsdatei auf den Benutzer, den du später festlegst
RUN chown 1654 /https/blazorapp.pfx && chmod 600 /https/blazorapp.pfx

# Wechsle nun zum nicht-Root-Benutzer
USER $APP_UID

# Umgebungsvariablen für HTTPS und Zertifikatspfad
ENV ASPNETCORE_URLS="https://+:8080"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/blazorapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=187test

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
ENTRYPOINT ["dotnet", "ChoiceVFileSystemBlazor.dll"]
