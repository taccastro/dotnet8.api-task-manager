using FluentAssertions;
using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;
using GerenciadorTarefas.API.Servicos;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace GerenciadorTarefas.Tests
{
    public class TarefaControllerTests
    {
        [Fact]
        public async Task CriarTarefa_DeveFuncionar()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            var dto = new TarefaCriarDto
            {
                Titulo = "Teste Criar",
                Descricao = "Descrição",
                Categoria = "Geral"
            };

            var tarefa = await service.CriarAsync(dto);

            var todas = await service.ListarAsync();
            todas.Should().HaveCount(1);
            todas.First().Titulo.Should().Be("Teste Criar");
        }

        [Fact]
        public async Task ListarTarefas_DeveFuncionar()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            await service.CriarAsync(new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "C1" });
            await service.CriarAsync(new TarefaCriarDto { Titulo = "T2", Descricao = "D2", Categoria = "C2" });

            var todas = await service.ListarAsync();
            todas.Should().HaveCount(2);
            todas.Select(t => t.Titulo).Should().Contain(new[] { "T1", "T2" });
        }

        [Fact]
        public async Task ObterPorId_DeveFuncionar()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            var tarefa = await service.CriarAsync(new TarefaCriarDto
            {
                Titulo = "TarefaX",
                Descricao = "DescX",
                Categoria = "Geral"
            });

            var encontrada = await service.ObterPorIdAsync(tarefa.Id);
            encontrada.Should().NotBeNull();
            encontrada!.Titulo.Should().Be("TarefaX");
        }

        [Fact]
        public async Task AtualizarTarefa_DeveFuncionar()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            var tarefa = await service.CriarAsync(new TarefaCriarDto
            {
                Titulo = "Old",
                Descricao = "Old",
                Categoria = "Geral"
            });

            var dtoAtualizar = new TarefaAtualizarDto
            {
                Titulo = "Updated",
                Descricao = "Updated",
                Categoria = "Geral"
            };

            var atualizada = await service.AtualizarAsync(tarefa.Id, dtoAtualizar);
            atualizada!.Titulo.Should().Be("Updated");
        }

        [Fact]
        public async Task DeletarTarefa_DeveFuncionar()
        {
            var repositorio = new TarefaRepositorioMemoria();
            var mockRabbitMQ = new Mock<IRabbitMQPublisher>();
            var service = new TarefaServico(repositorio, mockRabbitMQ.Object);

            var tarefa = await service.CriarAsync(new TarefaCriarDto
            {
                Titulo = "ToDelete",
                Descricao = "Desc",
                Categoria = "Geral"
            });

            var resultado = await service.DeletarAsync(tarefa.Id);
            resultado.Should().BeTrue();

            var todas = await service.ListarAsync();
            todas.Should().BeEmpty();
        }
    }
}
