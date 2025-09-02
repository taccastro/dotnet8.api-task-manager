# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo para dentro do container
COPY . .

# Restaura dependências
RUN dotnet restore

# Publica a aplicação
RUN dotnet publish -c Release -r linux-x64 --self-contained false -o /app/out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /app/out .

# Expõe a porta padrão (ajuste se necessário)
EXPOSE 8080

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "GerenciadorTarefas.API.dll"]