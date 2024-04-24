#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MotoDeliveryManager.RabbitMqConsumer/MotoDeliveryManager.RabbitMqConsumer.csproj", "MotoDeliveryManager.RabbitMqConsumer/"]
COPY ["MotoDeliveryManager.Core/MotoDeliveryManager.Domain.csproj", "MotoDeliveryManager.Core/"]
COPY ["MotoDeliveryManager.Infra/MotoDeliveryManager.Infra.csproj", "MotoDeliveryManager.Infra/"]
RUN dotnet restore "./MotoDeliveryManager.RabbitMqConsumer/MotoDeliveryManager.RabbitMqConsumer.csproj"
COPY . .
WORKDIR "/src/MotoDeliveryManager.RabbitMqConsumer"
RUN dotnet build "./MotoDeliveryManager.RabbitMqConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MotoDeliveryManager.RabbitMqConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MotoDeliveryManager.RabbitMqConsumer.dll"]