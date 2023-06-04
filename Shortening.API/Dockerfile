#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 61616
EXPOSE 61617

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:61617;http://+:61616
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/http/aspnetapp.pfx

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Shortening.API/Shortening.API.csproj", "Shortening.API/"]
COPY ["Shortening.API/aspnetapp.pfx", "Shortening.API/"]
RUN dotnet restore "Shortening.API/Shortening.API.csproj"
COPY . .
WORKDIR "/src/Shortening.API"
RUN dotnet build "Shortening.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shortening.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shortening.API.dll"]