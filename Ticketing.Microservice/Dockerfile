FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ./Ticketing.Microservice /src/Ticketing.Microservice
COPY ./Models /src/Models
RUN dotnet restore "Ticketing.Microservice/Ticketing.Microservice.csproj"

WORKDIR /src/Ticketing.Microservice
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing.Microservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:5000
ENV RABBITMQ_URI=rabbitmq://rabbitmq/
ENTRYPOINT ["dotnet", "Ticketing.Microservice.dll"]