FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./Sprungmarken.csproj" --disable-parallel
RUN dotnet publish "./Sprungmarken.csproj" -c release -o /app --no-restore



# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "Sprungmarken.dll"]
#docker build --rm -t seevogel/sprungmarken:latest .
#docker run --rm -p 5000:5000 -p 5001:5001 -v .\sprungmarken.txt:/app/sprungmarken.txt -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 seevogel/sprungmarken
