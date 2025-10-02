# Script para iniciar todo o ambiente Docker
Write-Host "🚀 Iniciando Gerenciador de Tarefas - Ambiente Completo" -ForegroundColor Green

# Para containers existentes (se houver)
Write-Host "🛑 Parando containers existentes..." -ForegroundColor Yellow
docker-compose down

# Remove imagens antigas (opcional)
Write-Host "🧹 Limpando imagens antigas..." -ForegroundColor Yellow
docker system prune -f

# Constrói e sobe todos os serviços
Write-Host "🔨 Construindo e iniciando todos os serviços..." -ForegroundColor Blue
docker-compose up --build -d

# Aguarda os serviços ficarem saudáveis
Write-Host "⏳ Aguardando serviços ficarem prontos..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Verifica status dos containers
Write-Host "📊 Status dos containers:" -ForegroundColor Cyan
docker-compose ps

# Mostra URLs de acesso
Write-Host "`n🌐 URLs de acesso:" -ForegroundColor Green
Write-Host "   API: http://localhost:8080" -ForegroundColor White
Write-Host "   Swagger: http://localhost:8080/swagger" -ForegroundColor White
Write-Host "   Health: http://localhost:8080/health" -ForegroundColor White
Write-Host "   Metrics: http://localhost:8080/metrics" -ForegroundColor White
Write-Host "   Prometheus: http://localhost:9090" -ForegroundColor White
Write-Host "   Grafana: http://localhost:3000 (admin/admin123)" -ForegroundColor White
Write-Host "   RabbitMQ Management: http://localhost:15672 (guest/guest)" -ForegroundColor White

Write-Host "`n✅ Ambiente iniciado! Use 'docker-compose logs -f api' para ver logs da API" -ForegroundColor Green
