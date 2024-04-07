#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/logistic/logistic.csproj", "src/logistic/"]
COPY ["src/Data/Data.csproj", "src/Data/"]
COPY ["src/Service/Services.csproj", "src/Service/"]
RUN dotnet restore "./src/logistic/./logistic.csproj"
COPY . .
WORKDIR "/src/src/logistic"
RUN dotnet build "./logistic.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./logistic.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "logistic.dll"]