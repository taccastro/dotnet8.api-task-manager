using FluentAssertions;
using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Servicos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GerenciadorTarefas.Tests
{
    public class TarefaControllerTests
    {
        private TarefaDbContext CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<TarefaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TarefaDbContext(options);
        }

        [Fact]
        public async Task CriarTarefa_DeveFuncionar()
        {
            using var db = CriarContextoEmMemoria();
            var servico = new TarefaServico(db);

            var dto = new TarefaCriarDto
            {
                Titulo = "Teste Criar",
                Descricao = "Descrição",
                Categoria = "Geral"
            };

            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria
            };

            db.Tarefas.Add(tarefa);
            await db.SaveChangesAsync();

            var todas = await db.Tarefas.ToListAsync();
            todas.Should().HaveCount(1);
            todas.First().Titulo.Should().Be("Teste Criar");
        }

        [Fact]
        public async Task ListarTarefas_DeveFuncionar()
        {
            using var db = CriarContextoEmMemoria();
            var servico = new TarefaServico(db);

            db.Tarefas.Add(new Tarefa { Titulo = "T1", Descricao = "D1", Categoria = "C1" });
            db.Tarefas.Add(new Tarefa { Titulo = "T2", Descricao = "D2", Categoria = "C2" });
            await db.SaveChangesAsync();

            var todas = await db.Tarefas.ToListAsync();
            todas.Should().HaveCount(2);
            todas.Select(t => t.Titulo).Should().Contain(new[] { "T1", "T2" });
        }

        [Fact]
        public async Task ObterPorId_DeveFuncionar()
        {
            using var db = CriarContextoEmMemoria();
            var tarefa = new Tarefa { Titulo = "TarefaX", Descricao = "DescX", Categoria = "Geral" };
            db.Tarefas.Add(tarefa);
            await db.SaveChangesAsync();

            var encontrada = await db.Tarefas.FindAsync(tarefa.Id);
            encontrada.Should().NotBeNull();
            encontrada!.Titulo.Should().Be("TarefaX");
        }

        [Fact]
        public async Task AtualizarTarefa_DeveFuncionar()
        {
            using var db = CriarContextoEmMemoria();
            var tarefa = new Tarefa { Titulo = "Old", Descricao = "Old", Categoria = "Geral" };
            db.Tarefas.Add(tarefa);
            await db.SaveChangesAsync();

            tarefa.Titulo = "Updated";
            db.Tarefas.Update(tarefa);
            await db.SaveChangesAsync();

            var atualizada = await db.Tarefas.FindAsync(tarefa.Id);
            atualizada!.Titulo.Should().Be("Updated");
        }

        [Fact]
        public async Task DeletarTarefa_DeveFuncionar()
        {
            using var db = CriarContextoEmMemoria();
            var tarefa = new Tarefa { Titulo = "ToDelete", Descricao = "Desc", Categoria = "Geral" };
            db.Tarefas.Add(tarefa);
            await db.SaveChangesAsync();

            db.Tarefas.Remove(tarefa);
            await db.SaveChangesAsync();

            var todas = await db.Tarefas.ToListAsync();
            todas.Should().BeEmpty();
        }
    }
}
