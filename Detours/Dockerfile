FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Detours/Detours.csproj", "Detours/"]
COPY ["Detours.Core/Detours.Core.csproj", "Detours.Core/"]
COPY ["Detours.Mediatr/Detours.Mediatr.csproj", "Detours.Mediatr/"]
COPY ["Detours.Services/Detours.Services.csproj", "Detours.Services/"]
COPY ["Detours.Data/Detours.Data.csproj", "Detours.Data/"]
RUN dotnet restore "Detours/Detours.csproj"
COPY . .
WORKDIR "/src/Detours"
RUN dotnet build "Detours.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Detours.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Detours.dll"]
