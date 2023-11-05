FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PdfGeneratorApp/PdfGeneratorApp.csproj", "PdfGeneratorApp/"]
RUN dotnet restore "PdfGeneratorApp/PdfGeneratorApp.csproj"
COPY . .
WORKDIR "/src/PdfGeneratorApp"
RUN dotnet build "PdfGeneratorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PdfGeneratorApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PdfGeneratorApp.dll"]
