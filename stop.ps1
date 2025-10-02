# Script para parar todo o ambiente Docker
Write-Host "🛑 Parando Gerenciador de Tarefas..." -ForegroundColor Yellow

# Para todos os containers
docker-compose down

# Remove volumes (opcional - descomente se quiser limpar dados)
# Write-Host "🧹 Removendo volumes..." -ForegroundColor Yellow
# docker-compose down -v

Write-Host "✅ Ambiente parado!" -ForegroundColor Green
