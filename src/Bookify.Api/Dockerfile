FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Bookify.Api/Bookify.Api.csproj", "src/Bookify.Api/"]
RUN dotnet restore "src/Bookify.Api/Bookify.Api.csproj"
COPY . .
WORKDIR "/src/src/Bookify.Api"
RUN dotnet build "Bookify.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookify.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookify.Api.dll"]