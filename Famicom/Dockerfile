# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY .. .
RUN rm /src/TableControllerAPI/appsettings.Development.json
RUN rm /src/TableControllerAPI/appsettings.json

RUN dotnet restore "./Famicom/Famicom.csproj"
WORKDIR "/src/"
RUN dotnet build "Famicom/Famicom.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Famicom/Famicom.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Release
ENV DISABLE_HTTPS_REDIRECTION=true
ENTRYPOINT ["dotnet", "Famicom.dll"]