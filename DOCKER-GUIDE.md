# 🐳 Guia Docker - Gerenciador de Tarefas

## 🚀 Como rodar tudo em containers

### Pré-requisitos
- Docker Desktop instalado
- PowerShell (Windows) ou Terminal (Linux/Mac)

### 1) Iniciar tudo
```powershell
# Opção 1: Script automático (recomendado)
.\start.ps1

# Opção 2: Manual
docker-compose up --build -d
```

### 2) Verificar se tudo subiu
```powershell
docker-compose ps
```
Todos os serviços devem estar "Up" e "healthy".

### 3) URLs de acesso
- **API**: http://localhost:8080
- **Swagger**: http://localhost:8080/swagger
- **Health Check**: http://localhost:8080/health
- **Métricas**: http://localhost:8080/metrics
- **Prometheus**: http://localhost:9090
- **Grafana**: http://localhost:3000 (admin/admin123)
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)

## 🧪 Como testar

### 1) Teste básico da API
```powershell
# Health check
curl http://localhost:8080/health

# Listar tarefas
curl http://localhost:8080/api/Tarefa

# Métricas
curl http://localhost:8080/metrics
```

### 2) Teste via Swagger
1. Abra http://localhost:8080/swagger
2. Teste os endpoints de tarefas
3. Observe as métricas no Prometheus

### 3) Teste do Prometheus
1. Abra http://localhost:9090
2. Vá em Status → Targets
3. Verifique se `gerenciador_tarefas_api` está UP
4. Teste consultas:
```promql
# Total de requisições
sum(rate(http_requests_received_total[1m]))

# Por rota
sum by (route) (rate(http_requests_received_total[1m]))

# Latência p95
histogram_quantile(0.95, sum by (le, route) (rate(http_request_duration_seconds_bucket[5m])))
```

### 4) Teste do Grafana
1. Abra http://localhost:3000
2. Login: admin/admin123
3. Adicione data source Prometheus: http://prometheus:9090
4. Crie dashboard com as consultas acima

### 5) Teste do RabbitMQ
1. Abra http://localhost:15672
2. Login: guest/guest
3. Vá em Queues → "tarefas"
4. Faça requisições na API e veja mensagens chegando

## 🔧 Comandos úteis

### Ver logs
```powershell
# Todos os serviços
docker-compose logs -f

# Apenas API
docker-compose logs -f api

# Apenas Prometheus
docker-compose logs -f prometheus
```

### Parar tudo
```powershell
# Opção 1: Script
.\stop.ps1

# Opção 2: Manual
docker-compose down
```

### Limpar tudo (remove dados)
```powershell
docker-compose down -v
docker system prune -f
```

### Rebuild apenas a API
```powershell
docker-compose up --build -d api
```

## 🐛 Troubleshooting

### API não sobe
```powershell
# Ver logs da API
docker-compose logs api

# Verificar se dependências estão saudáveis
docker-compose ps
```

### Prometheus não coleta métricas
1. Verifique se API está UP: http://localhost:8080/health
2. Teste métricas: http://localhost:8080/metrics
3. No Prometheus: Status → Targets → verifique se está UP

### Grafana não conecta no Prometheus
1. Data source URL deve ser: `http://prometheus:9090` (não localhost)
2. Teste a conexão no Grafana

### Banco não conecta
```powershell
# Ver logs do Postgres
docker-compose logs postgres

# Conectar no banco
docker exec -it gerenciador-postgres psql -U postgres -d GerenciadorTarefas
```

## 📊 Monitoramento

### Métricas importantes
- **RPS**: `sum(rate(http_requests_received_total[1m]))`
- **Latência p95**: `histogram_quantile(0.95, sum by (le) (rate(http_request_duration_seconds_bucket[5m])))`
- **Erros 5xx**: `sum(rate(http_requests_received_total{code=~"5.."}[1m]))`
- **Memória .NET**: `dotnet_total_memory_bytes`
- **GC**: `dotnet_collection_count_total`

### Alertas sugeridos
- RPS > 100 req/s
- Latência p95 > 500ms
- Erros 5xx > 1%
- Memória > 100MB

## 🎯 Próximos passos
1. Configurar alertas no Prometheus
2. Criar dashboards no Grafana
3. Implementar health checks mais robustos
4. Adicionar tracing (Jaeger)
5. Configurar CI/CD com Docker
