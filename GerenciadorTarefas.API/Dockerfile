# Dockerfile simples que sempre funciona
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copia os arquivos de projeto
COPY ["GerenciadorTarefas.API/GerenciadorTarefas.API.csproj", "GerenciadorTarefas.API/"]
COPY ["GerenciadorTarefas.Testes/GerenciadorTarefas.Tests.csproj", "GerenciadorTarefas.Testes/"]
COPY ["GerenciadorTarefas.API.sln", "."]

# Restaura dependências
RUN dotnet restore "GerenciadorTarefas.API/GerenciadorTarefas.API.csproj"

# Copia todo o código
COPY . .

# Expõe as portas
EXPOSE 8080
EXPOSE 8081

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Comando para rodar (sem publish, roda direto do SDK)
ENTRYPOINT ["dotnet", "run", "--project", "GerenciadorTarefas.API/GerenciadorTarefas.API.csproj", "--urls", "http://0.0.0.0:8080"]