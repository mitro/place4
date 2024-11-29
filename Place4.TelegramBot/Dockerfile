FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY ./Place4.TelegramBot/Place4.TelegramBot.csproj ./Place4.TelegramBot/
RUN dotnet restore ./Place4.TelegramBot/Place4.TelegramBot.csproj

COPY . .
RUN dotnet build ./Place4.TelegramBot/Place4.TelegramBot.csproj -c Release -o /app/build

RUN dotnet publish ./Place4.TelegramBot/Place4.TelegramBot.csproj -c Release -o /app/publish

FROM base AS final

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Place4.TelegramBot.dll"]
