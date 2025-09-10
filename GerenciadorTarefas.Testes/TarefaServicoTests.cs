using FluentAssertions;
using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;
using GerenciadorTarefas.API.Servicos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace GerenciadorTarefas.Tests
{
    public class TarefaServicoTests
    {
        [Fact]
        public async Task CriarAsync_DeveAdicionarTarefa()
        {
            // Arrange
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            var dto = new TarefaCriarDto
            {
                Titulo = "Teste",
                Descricao = "Descri��o",
                Categoria = "Geral"
            };

            // Act
            var tarefa = await service.CriarAsync(dto);

            // Assert
            var tarefasNoDb = await repositorio.ListarTodasTarefas();
            tarefasNoDb.Count.Should().Be(1);
            tarefa.Titulo.Should().Be("Teste");
            tarefa.Descricao.Should().Be("Descri��o");
            tarefa.Categoria.Should().Be("Geral");
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarTodasTarefas()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            await service.CriarAsync(new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "G1" });
            await service.CriarAsync(new TarefaCriarDto { Titulo = "T2", Descricao = "D2", Categoria = "G2" });

            var lista = await service.ListarAsync();
            lista.Count.Should().Be(2);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarTarefa()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            var dto = new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "G1" };
            var criada = await service.CriarAsync(dto);

            var tarefa = await service.ObterPorIdAsync(criada.Id);
            tarefa.Should().NotBeNull();
            tarefa!.Id.Should().Be(criada.Id);
        }

        [Fact]
        public async Task DeletarAsync_DeveRemoverTarefa()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            var dto = new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "G1" };
            var criada = await service.CriarAsync(dto);

            var resultado = await service.DeletarAsync(criada.Id);
            resultado.Should().BeTrue();

            var lista = await service.ListarAsync();
            lista.Count.Should().Be(0);
        }
    }
}
