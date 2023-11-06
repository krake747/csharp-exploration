FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HtmxApp/Htmx.Api/Htmx.Api.csproj", "HtmxApp/Htmx.Api/"]
RUN dotnet restore "HtmxApp/Htmx.Api/Htmx.Api.csproj"
COPY . .
WORKDIR "/src/HtmxApp/Htmx.Api"
RUN dotnet build "Htmx.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Htmx.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Htmx.Api.dll"]
