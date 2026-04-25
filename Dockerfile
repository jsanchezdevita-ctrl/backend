# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar solución y proyectos
COPY *.sln ./
COPY src ./src
COPY test ./test

# Restaurar paquetes
RUN dotnet restore

# Publicar solo el proyecto de la API
RUN dotnet publish src/Web.Api/Web.Api.csproj -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/out ./

# Puerto que escucha .NET (Railway usa la variable PORT)
EXPOSE 5000

# Ejecutar la API
ENTRYPOINT ["dotnet", "Web.Api.dll"]