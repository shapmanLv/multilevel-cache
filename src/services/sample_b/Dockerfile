#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/sample_b/sample_b.csproj", "services/sample_b/"]
RUN dotnet restore "services/sample_b/sample_b.csproj"
COPY . .
WORKDIR "/src/services/sample_b"
RUN dotnet build "sample_b.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sample_b.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sample_b.dll"]
