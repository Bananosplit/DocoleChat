FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /App
COPY proto ./proto/

WORKDIR /App/backend/irc-backend
COPY backend/irc-backend ./
EXPOSE 5085
RUN dotnet publish -o out
WORKDIR /App/backend/irc-backend/out
ENTRYPOINT ["dotnet", "irc-backend.dll"]
#RUN dotnet build -o out

#FROM mcr.microsoft.com/dotnet/aspnet:9.0
#WORKDIR /App/backend/irc-backend
#COPY --from=build /backend/irc-backend/out .
#WORKDIR /App/backend/irc-backend/out
#ENTRYPOINT ["dotnet", "irc-server.dll"]
