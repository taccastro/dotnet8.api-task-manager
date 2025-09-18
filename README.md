# Gerenciador de Tarefas

![.NET 8](https://img.shields.io/badge/.NET-8-blue)
![Async/Await](https://img.shields.io/badge/Async--Await-green)
![SOLID](https://img.shields.io/badge/SOLID-purple)
![Docker](https://img.shields.io/badge/Docker-lightgrey)
![Kubernetes](https://img.shields.io/badge/Kubernetes-lightblue)
![Redis](https://img.shields.io/badge/Redis-orange)

> API simples para gerenciar tarefas. Feita para praticar .NET 8, arquitetura limpa e algumas ferramentas legais do ecossistema.

---

## Objetivo

A ideia do projeto é criar uma API que seja fácil de entender e manter, usando .NET 8 e boas práticas de programação. Queria experimentar coisas como Docker, RabbitMQ, Redis, PostgreSQL, Prometheus e Grafana, mas de um jeito que eu pudesse testar e aprender sem complicar demais.

---

## O que já está funcionando

* CRUD completo de tarefas (criar, listar, buscar, atualizar e remover)
* Organização por categorias e prioridades
* Busca de tarefas por categoria ou prioridade
* Métodos e DTOs em português
* Testes unitários básicos para controllers e serviços
* Middleware para tratamento de erros de forma centralizada
* Swagger/OpenAPI configurado para documentação
* Estrutura inicial de autenticação JWT
* Cache Redis para deixar listagens mais rápidas
* RabbitMQ publicando eventos quando tarefas são criadas, atualizadas ou removidas
* Logs básicos armazenados no MongoDB
* Arquitetura organizada em Controllers, Serviços e Repositórios 

---

## Próximos passos

* [ ] Prometheus & Grafana: monitorar a API e criar dashboards
* [ ] Nginx: configurar reverse proxy para produção
* [ ] Testes de integração ponta a ponta
* [ ] Docker/Kubernetes: preparar para deploy real

---

## Tecnologias usadas

| Ferramenta           | Status     | Uso no Projeto                       |
| -------------------- | ---------- | ------------------------------------ |
| .NET 8 / C#          | ✅ Feito    | API e serviços                       |
| PostgreSQL           | ✅ Feito    | Banco de dados                       |
| Docker               | ⚠️ Parcial | Conteinerização (falta documentação) |
| Kubernetes           | ⏳ Pendente | Orquestração                         |
| Redis                | ✅ Feito    | Cache distribuído                    |
| RabbitMQ             | ✅ Feito    | Mensageria básica                    |
| MongoDB              | ✅ Feito    | Logs / histórico                     |
| Nginx                | ⏳ Pendente | Reverse proxy                        |
| Prometheus & Grafana | ⏳ Pendente | Monitoramento                        |

---

## Endpoints principais

* `GET /api/tarefa` → Lista todas as tarefas
* `GET /api/tarefa/{id}` → Busca tarefa por ID
* `POST /api/tarefa` → Cria nova tarefa
* `PUT /api/tarefa/{id}` → Atualiza tarefa
* `DELETE /api/tarefa/{id}` → Remove tarefa
* `GET /api/tarefa/filtro` → Filtra tarefas por categoria, prioridade, página e ordenação

Exemplo de filtro:

```
GET /api/tarefa/filtro?categoria=Trabalho&prioridade=3&pagina=1&tamanhoPagina=10&ordenarPor=Prioridade&desc=true
```

---

## Como rodar

1. Tenha instalado:

   * [.NET 8 SDK](https://dotnet.microsoft.com/download)
   * [Docker](https://www.docker.com/) (para Redis, RabbitMQ e MongoDB)

2. Configure o PostgreSQL e atualize `appsettings.json`:

```json
"PostgreSQL": {
  "ConnectionString": "Host=localhost;Database=GerenciadorTarefas;Username=postgres;Password=senha"
}
```

3. Rode o projeto:

```bash
dotnet restore
dotnet build
dotnet run
```

4. Abra o Swagger: `https://localhost:{porta}/swagger`

---

## Observações

O projeto já tem Redis e RabbitMQ funcionando e logs básicos no MongoDB. É feito de forma que fique fácil adicionar mais funcionalidades, monitoramento e deploy em Docker/Kubernetes no futuro.
