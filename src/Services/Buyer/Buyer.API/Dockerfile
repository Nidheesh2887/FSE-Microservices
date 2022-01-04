#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Buyer/Buyer.API/Buyer.API.csproj", "Services/Buyer/Buyer.API/"]
COPY ["MessagingBlocks/Messaging.AzureBus/Messaging.AzureBus.csproj", "MessagingBlocks/Messaging.AzureBus/"]
RUN dotnet restore "Services/Buyer/Buyer.API/Buyer.API.csproj"
COPY . .
WORKDIR "/src/Services/Buyer/Buyer.API"
RUN dotnet build "Buyer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Buyer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Buyer.API.dll"]