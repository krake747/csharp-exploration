FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HtmxApp/Htmx.App/Htmx.App.csproj", "HtmxApp/Htmx.App/"]
RUN dotnet restore "HtmxApp/Htmx.App/Htmx.App.csproj"
COPY . .
WORKDIR "/src/HtmxApp/Htmx.App"
RUN dotnet build "Htmx.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Htmx.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Htmx.App.dll"]
