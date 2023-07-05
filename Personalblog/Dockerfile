#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5031
EXPOSE 7031

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY fonts/ /usr/share/fonts/dejavu/

COPY . .

ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT ["dotnet", "Personalblog.dll"]
