FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ./TicketProcessor.Microservice /src/TicketProcessor.Microservice
COPY ./Models /src/Models
RUN dotnet restore "TicketProcessor.Microservice/TicketProcessor.Microservice.csproj"

WORKDIR /src/TicketProcessor.Microservice
RUN ls
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketProcessor.Microservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:6000
ENV RABBITMQ_URI=rabbitmq://rabbitmq/
ENTRYPOINT ["dotnet", "TicketProcessor.Microservice.dll"]