# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todo el código
COPY . .

# Restaurar dependencias y publicar
RUN dotnet restore WebSastreria/WebSastreria.csproj
RUN dotnet publish WebSastreria/WebSastreria.csproj -c Release -o out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Exponer el puerto
EXPOSE 8080

# Comando de inicio
ENTRYPOINT ["dotnet", "WebSastreria.dll"]
