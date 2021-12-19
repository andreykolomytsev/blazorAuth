#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["Client/Client.csproj", "Client/"]
#COPY ["Client.Infrastructure/Client.Infrastructure.csproj", "Client.Infrastructure/"]
#COPY ["Shared/Shared.csproj", "Shared/"]
#RUN dotnet restore "Client/Client.csproj"
#COPY . .
#WORKDIR "/src/Client"
#RUN dotnet build "Client.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Client.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "AuthClient.Client.dll"]




FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app


COPY ["Client.Infrastructure/Client.Infrastructure.csproj", "Client.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]

COPY ["Client/Client.csproj", "Client/"]

COPY . ./
RUN dotnet publish -c Release -o output
FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build-env /app/output/wwwroot .
COPY Client/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80