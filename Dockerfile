FROM grpc/csharp
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App
COPY proto ./proto/

WORKDIR /App/backend/irc-backend
COPY backend/irc-backend ./
#RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build /App/backend/irc-backend/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "irc-backend.dll", "--server.urls", "https://+:5000"]
