FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BlogApi/BlogApi.csproj", "BlogApi/"]
RUN dotnet restore "BlogApi/BlogApi.csproj"
COPY . .
WORKDIR "/src/BlogApi"
RUN dotnet build "BlogApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogApi.dll"]
