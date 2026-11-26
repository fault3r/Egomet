FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Egomet.slnx", "."]
COPY ["src/Egomet.Api/Egomet.Api.csproj", "src/Egomet.Api/"]
COPY ["src/Egomet.Infrastructure/Egomet.Infrastructure.csproj", "src/Egomet.Infrastructure/"]
COPY ["src/Egomet.Application/Egomet.Application.csproj", "src/Egomet.Application/"]
COPY ["src/Egomet.Domain/Egomet.Domain.csproj", "src/Egomet.Domain/"]
RUN dotnet restore "Egomet.slnx"
COPY src/ ./src/
RUN dotnet build "Egomet.slnx" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Egomet.Api/Egomet.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5012
ENTRYPOINT ["dotnet", "Egomet.Api.dll"]