FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY blazorapp.pfx /https/blazorapp.pfx

ENV ASPNETCORE_URLS="https://+:8080"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/blazorapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=187test

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChoiceVFileSystemBlazor/ChoiceVFileSystemBlazor.csproj", "ChoiceVFileSystemBlazor/"]
COPY ["ChoiceVApi/ChoiceVApi.csproj", "ChoiceVApi/"]
COPY ["ChoiceVSharedApiModels/ChoiceVSharedApiModels.csproj", "ChoiceVSharedApiModels/"]
COPY ["ChoiceVRefitClient/ChoiceVRefitClient.csproj", "ChoiceVRefitClient/"]
RUN dotnet restore "ChoiceVFileSystemBlazor/ChoiceVFileSystemBlazor.csproj"
COPY . .
WORKDIR "/src/ChoiceVFileSystemBlazor"
RUN dotnet build "ChoiceVFileSystemBlazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChoiceVFileSystemBlazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChoiceVFileSystemBlazor.dll"]
