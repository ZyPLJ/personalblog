#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5031
EXPOSE 7031

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
#COPY ["Personalblog/Personalblog.csproj", "Personalblog/"]
#COPY ["Personalblog.Migrate/Personalblog.Migrate.csproj", "Personalblog.Migrate/"]
#COPY ["Personalblog.Model/Personalblog.Model.csproj", "Personalblog.Model/"]
#COPY ["PersonalblogServices/PersonalblogServices.csproj", "PersonalblogServices/"]
#RUN dotnet restore "Personalblog/Personalblog.csproj"
#COPY . .
#WORKDIR "/src/Personalblog"
#RUN dotnet build "Personalblog.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Personalblog.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Personalblog.dll"]