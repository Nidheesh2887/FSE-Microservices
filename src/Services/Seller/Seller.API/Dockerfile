#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Seller/Seller.API/Seller.API.csproj", "Services/Seller/Seller.API/"]
COPY ["MessagingBlocks/Messaging.AzureBus/Messaging.AzureBus.csproj", "MessagingBlocks/Messaging.AzureBus/"]
RUN dotnet restore "Services/Seller/Seller.API/Seller.API.csproj"
COPY . .
WORKDIR "/src/Services/Seller/Seller.API"
RUN dotnet build "Seller.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seller.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seller.API.dll"]