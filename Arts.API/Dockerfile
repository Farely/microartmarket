FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Arts.API/Arts.API.csproj", "Arts.API/"]
RUN dotnet restore "Arts.API/Arts.API.csproj"
COPY . .
WORKDIR "/src/Arts.API"
RUN dotnet build "Arts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Arts.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arts.API.dll"]
