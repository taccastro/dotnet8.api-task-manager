using FluentAssertions;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;
using GerenciadorTarefas.API.Servicos;
using Moq;
using Xunit;

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
        [Fact]        public async Task BuscarPorFiltrosAsync_DeveRetornarCorretamente()
        {
            // Arrange
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            // Criar tarefas de teste
            await service.CriarAsync(new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "C1", Prioridade = 1 });
            await service.CriarAsync(new TarefaCriarDto { Titulo = "T2", Descricao = "D2", Categoria = "C2", Prioridade = 2 });
            await service.CriarAsync(new TarefaCriarDto { Titulo = "T3", Descricao = "D3", Categoria = "C1", Prioridade = 3 });
            await service.CriarAsync(new TarefaCriarDto { Titulo = "T4", Descricao = "D4", Categoria = "C2", Prioridade = 1 });

            // Act
            var resultadoCategoria = await service.BuscarPorFiltrosAsync("C1", null, 1, 20);
            var resultadoPrioridade = await service.BuscarPorFiltrosAsync(null, 1, 1, 20);
            var resultadoAmbos = await service.BuscarPorFiltrosAsync("C2", 1, 1, 20);

            // Assert
            resultadoCategoria.Should().HaveCount(2);
            resultadoCategoria.Select(t => t.Titulo).Should().Contain(new[] { "T1", "T3" });

            resultadoPrioridade.Should().HaveCount(2);
            resultadoPrioridade.Select(t => t.Titulo).Should().Contain(new[] { "T1", "T4" });

            resultadoAmbos.Should().HaveCount(1);
            resultadoAmbos.First().Titulo.Should().Be("T4");
        }
    }
}
