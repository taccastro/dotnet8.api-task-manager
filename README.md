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

##  Funcionalidades já implementadas

- CRUD completo de tarefas (criar, listar, buscar, atualizar, remover)
- Organização por categorias e prioridades
- Busca de tarefas por prioridade ou categoria
- Métodos e DTOs em português
- Testes unitários para controllers e serviços
- Middleware para tratamento global de exceções
- Documentação automática via Swagger/OpenAPI
- Estrutura inicial para autenticação e notificações (em desenvolvimento)
- Cache Redis implementado para otimizar buscas e listagens
- RabbitMQ: eventos básicos já implementados (Publisher e Consumer)
- MongoDB: logs básicos
---

## 🚧 Próximos passos e integrações planejadas

- [x] **Redis:** Cache implementado e funcionando
- [x] **RabbitMQ:** Publicar eventos ao criar, atualizar ou remover tarefas
- [x] **MongoDB:** Armazenar logs ou histórico de alterações
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
| RabbitMQ              | ✅ Pronto  | Mensageria (a implementar)            |
| MongoDB               | ✅ Pronto | Logs/histórico (a implementar)        |
| Nginx                 | ⏳ Pendente | Reverse proxy (a documentar)          |
| Prometheus & Grafana  | ⏳ Pendente | Monitoramento (a implementar)         |

---

##  Como rodar o projeto

1. **Pré-requisitos:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/) (RabbitMQ / REDIS / MONGO)

2. **Configuração do banco:**
   - O projeto usa PostgreSQL via Entity Framework Core.
   - Ajuste a connection string em `appsettings.json`:


