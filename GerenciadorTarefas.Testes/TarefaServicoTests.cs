using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Servicos;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GerenciadorTarefas.Tests
{
    public class TarefaServicoTests
    {
        // Cria um DbContext em memória para os testes
        private TarefaDbContext ObterContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<TarefaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TarefaDbContext(options);
        }

        [Fact]
        public async Task CriarAsync_DeveAdicionarTarefa()
        {
            // Arrange
            var db = ObterContextoEmMemoria();
            var service = new TarefaServico(db);

            var dto = new TarefaCriarDto
            {
                Titulo = "Teste",
                Descricao = "Descrição",
                Categoria = "Geral"
            };

            // Act
            var tarefa = await service.CriarAsync(dto);

            // Assert
            var tarefasNoDb = await db.Tarefas.CountAsync();
            tarefasNoDb.Should().Be(1);
            tarefa.Titulo.Should().Be("Teste");
            tarefa.Descricao.Should().Be("Descrição");
            tarefa.Categoria.Should().Be("Geral");
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarTodasTarefas()
        {
            // Arrange
            var db = ObterContextoEmMemoria();
            var service = new TarefaServico(db);

            await service.CriarAsync(new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "G1" });
            await service.CriarAsync(new TarefaCriarDto { Titulo = "T2", Descricao = "D2", Categoria = "G2" });

            // Act
            var lista = await service.ListarAsync();

            // Assert
            lista.Count.Should().Be(2);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarTarefa()
        {
            // Arrange
            var db = ObterContextoEmMemoria();
            var service = new TarefaServico(db);

            var dto = new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "G1" };
            var criada = await service.CriarAsync(dto);

            // Act
            var tarefa = await service.ObterPorIdAsync(criada.Id);

            // Assert
            tarefa.Should().NotBeNull();
            tarefa!.Id.Should().Be(criada.Id);
        }

        [Fact]
        public async Task DeletarAsync_DeveRemoverTarefa()
        {
            // Arrange
            var db = ObterContextoEmMemoria();
            var service = new TarefaServico(db);

            var dto = new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "G1" };
            var criada = await service.CriarAsync(dto);

            // Act
            var resultado = await service.DeletarAsync(criada.Id);

            // Assert
            resultado.Should().BeTrue();
            var lista = await service.ListarAsync();
            lista.Count.Should().Be(0);
        }
    }
}
