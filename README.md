# Gerenciador de Tarefas

![Badge](https://img.shields.io/badge/.NET-8-blue)
![Badge](https://img.shields.io/badge/Async--Await-green)
![Badge](https://img.shields.io/badge/SOLID-purple)
![Badge](https://img.shields.io/badge/Docker-lightgrey)
![Badge](https://img.shields.io/badge/Kubernetes-lightblue)
![Badge](https://img.shields.io/badge/Redis-orange)

> API para gerenciamento de tarefas, desenvolvida para praticar e experimentar tecnologias modernas do ecossistema .NET.

---

## 🎯 Objetivo

Este projeto nasceu com a ideia de criar uma API escalável e testável utilizando .NET 8, aplicando princípios SOLID, Clean Architecture e programação assíncrona. Além disso, estou integrando ferramentas como Docker, Kubernetes, RabbitMQ, Redis, PostgreSQL, Prometheus e Grafana, tanto para aprendizado quanto para montar uma base sólida para projetos futuros.

---

## ⚡ Funcionalidades já implementadas

- CRUD completo de tarefas (criar, listar, buscar, atualizar, remover)
- Organização por categorias e prioridades
- Busca de tarefas por prioridade ou categoria
- Métodos e DTOs em português
- Testes unitários para controllers e serviços
- Middleware para tratamento global de exceções
- Documentação automática via Swagger/OpenAPI
- Estrutura inicial para autenticação e notificações (em desenvolvimento)
- **Cache Redis implementado** para otimizar buscas e listagens

---

## 🚧 Próximos passos e integrações planejadas

- [x] **Redis:** Cache implementado e funcionando
- [ ] **RabbitMQ:** Publicar eventos ao criar, atualizar ou remover tarefas
- [ ] **MongoDB:** Armazenar logs ou histórico de alterações
- [ ] **Prometheus & Grafana:** Expor métricas da API e criar dashboards de monitoramento
- [ ] **Nginx:** Configurar como reverse proxy para produção
- [ ] **Autenticação/Autorização:** Implementar JWT ou OAuth2
- [ ] **Testes de Integração:** Cobrir cenários ponta a ponta
- [ ] **Deploy com Docker/Kubernetes:** Finalizar arquivos de configuração e documentação

---

## 🛠 Tecnologias

| Ferramenta            | Status      | Uso no Projeto                        |
|-----------------------|-------------|---------------------------------------|
| .NET 8 / C#           | ✅ Pronto   | API e serviços                        |
| PostgreSQL            | ✅ Pronto   | Banco de dados relacional             |
| Docker                | ⚠️ Parcial  | Conteinerização (falta documentação)  |
| Kubernetes            | ⏳ Pendente | Orquestração (a testar/documentar)    |
| Redis                 | ✅ Pronto   | Cache distribuído para otimização     |
| RabbitMQ              | ⏳ Pendente | Mensageria (a implementar)            |
| MongoDB               | ⏳ Pendente | Logs/histórico (a implementar)        |
| Nginx                 | ⏳ Pendente | Reverse proxy (a documentar)          |
| Prometheus & Grafana  | ⏳ Pendente | Monitoramento (a implementar)         |

---

## 🚀 Como rodar o projeto

1. **Pré-requisitos:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/) (opcional, para banco e Redis)

2. **Configuração do banco:**
   - O projeto está configurado para usar PostgreSQL via Entity Framework Core.
   - Ajuste a connection string em `appsettings.json`:

```json
"ConnectionStrings": {
  "BancoPostgreSQL": "Host=localhost;Port=5432;Database=GerenciadorTarefas;Username=postgres;Password=123456"
}

Configuração do Redis:

Ajuste em appsettings.json:

"Redis": {
  "Servidor": "localhost",
  "Porta": 6379
}

Para rodar via Docker (opcional):
docker run -d --name redis-local -p 6379:6379 redis:latest


Executando a API:
dotnet build
dotnet run


Testando Redis:

Faça um GET /tarefas:

Primeira vez: dados vêm do banco, cache será preenchido.

Console mostrará: Buscando tarefas no banco...

Faça outro GET /tarefas:

Dados são retornados do Redis.

Console mostrará: Retornando tarefas do cache

Adicione/Atualize/Remova tarefas:

Cache global e individual é invalidado.

Próximo GET vai buscar no banco novamente e atualizar o cache.

Para debug: use forcarRefresh = true no método ListarTodasTarefas() para ignorar o cache.

