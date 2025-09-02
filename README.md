# Gerenciador de Tarefas

![Badge](https://img.shields.io/badge/.NET-8-blue)
![Badge](https://img.shields.io/badge/Async--Await-green)
![Badge](https://img.shields.io/badge/SOLID-purple)
![Badge](https://img.shields.io/badge/Docker-lightgrey)
![Badge](https://img.shields.io/badge/Kubernetes-lightblue)

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

---

## 🚧 Próximos passos e integrações planejadas

- [ ] **Redis:** Implementar cache para otimizar buscas e listagens
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
| Redis                 | ⏳ Pendente | Cache (a implementar)                 |
| RabbitMQ              | ⏳ Pendente | Mensageria (a implementar)            |
| MongoDB               | ⏳ Pendente | Logs/histórico (a implementar)        |
| Nginx                 | ⏳ Pendente | Reverse proxy (a documentar)          |
| Prometheus & Grafana  | ⏳ Pendente | Monitoramento (a implementar)         |


---

## 🚀 Como rodar o projeto

1. **Pré-requisitos:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/) (opcional, para banco e serviços externos)

2. **Configuração do banco:**
   - O projeto está configurado para usar PostgreSQL via Entity Framework Core.
   - Para rodar localmente, ajuste a connection string em `appsettings.json`.

3. **Executando a API:**
   ```bash
   dotnet build
   dotnet run