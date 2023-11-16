FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["grpc/Bikes.Grpc/Bikes.Grpc.csproj", "grpc/Bikes.Grpc/"]
RUN dotnet restore "grpc/Bikes.Grpc/Bikes.Grpc.csproj"
COPY . .
WORKDIR "/src/grpc/Bikes.Grpc"
RUN dotnet build "Bikes.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bikes.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bikes.Grpc.dll"]
