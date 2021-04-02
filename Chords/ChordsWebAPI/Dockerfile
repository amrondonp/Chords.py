#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ChordsWebAPI/ChordsWebAPI.csproj", "ChordsWebAPI/"]
RUN dotnet restore "ChordsWebAPI/ChordsWebAPI.csproj"
COPY . .
WORKDIR "/src/ChordsWebAPI"
RUN dotnet build "ChordsWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChordsWebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChordsWebAPI.dll"]