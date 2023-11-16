FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["grpc/Bikes.Consumer/Bikes.Consumer.csproj", "grpc/Bikes.Consumer/"]
RUN dotnet restore "grpc/Bikes.Consumer/Bikes.Consumer.csproj"
COPY . .
WORKDIR "/src/grpc/Bikes.Consumer"
RUN dotnet build "Bikes.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bikes.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bikes.Consumer.dll"]
