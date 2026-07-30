# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Products.csproj", "./"]
RUN dotnet restore "./Products.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
# Las apps de .NET recientes usan el puerto 8080 por defecto en contenedores
EXPOSE 8080 
ENTRYPOINT ["dotnet", "Products.dll"]