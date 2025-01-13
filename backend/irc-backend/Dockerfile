FROM mcr.microsoft.com/dotnet/sdk:9.0
COPY . ./

RUN dotnet restore

RUN dotnet run
RUN dotnet publish -o out