FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GenericLicensing.Core/GenericLicensing.Core.csproj", "GenericLicensing.Core/"]
RUN dotnet restore "GenericLicensing.Core/GenericLicensing.Core.csproj"
COPY . .
WORKDIR "/src/GenericLicensing.Core"
RUN dotnet build "GenericLicensing.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GenericLicensing.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GenericLicensing.Core.dll"]
