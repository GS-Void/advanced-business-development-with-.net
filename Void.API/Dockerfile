# Fase de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia o csproj e restaura as dependências
COPY ["Void.API.csproj", "./"]
RUN dotnet restore "./Void.API.csproj"

# Copia todo o resto do código e compila a aplicação
COPY . .
RUN dotnet publish "Void.API.csproj" -c Release -o /app/publish

# Fase de Execução
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development
ENV TZ=America/Sao_Paulo

# Copia os arquivos compilados da fase de build
COPY --from=build /app/publish .

EXPOSE 8080
USER app

# Comando de inicialização
ENTRYPOINT ["dotnet", "Void.API.dll"]