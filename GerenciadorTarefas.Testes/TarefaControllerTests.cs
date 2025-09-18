using FluentAssertions;
using GerenciadorTarefas.API;
using GerenciadorTarefas.API.Modelos.Dados;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace GerenciadorTarefas.Tests
{
    // Garantir que o WebApplicationFactory aponte para o Program do projeto API
    public class TarefaControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TarefaControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Cria client direto do projeto API
            _client = factory.CreateClient();
        }

        // Limpar todas as tarefas antes de cada teste
        private async Task LimparBaseAsync()
        {
            var listarResp = await _client.GetAsync("/tarefa");
            if (listarResp.StatusCode == HttpStatusCode.OK)
            {
                var tarefas = await listarResp.Content.ReadFromJsonAsync<List<TarefaDto>>();
                if (tarefas is not null)
                {
                    foreach (var t in tarefas)
                        await _client.DeleteAsync($"/tarefa/{t.Id}");
                }
            }
        }

        [Fact]
        public async Task CriarEListarTarefas_DeveFuncionar()
        {
            await LimparBaseAsync();

            var dto = new TarefaCriarDto
            {
                Titulo = "Teste Controller",
                Descricao = "Descrição Controller",
                Categoria = "Geral",
                Prioridade = 2
            };

            var criarResp = await _client.PostAsJsonAsync("/tarefa", dto);
            criarResp.EnsureSuccessStatusCode();

            var listarResp = await _client.GetAsync("/tarefa");
            listarResp.EnsureSuccessStatusCode();

            var tarefas = await listarResp.Content.ReadFromJsonAsync<List<TarefaDto>>();
            tarefas.Should().HaveCount(1);
            tarefas.First().Titulo.Should().Be("Teste Controller");
        }

        [Fact]
        public async Task ObterAtualizarEDeletarTarefa_DeveFuncionar()
        {
            await LimparBaseAsync();

            // Criar
            var criarDto = new TarefaCriarDto { Titulo = "TarefaX", Descricao = "DescX", Categoria = "Geral", Prioridade = 1 };
            var criarResp = await _client.PostAsJsonAsync("/tarefa", criarDto);
            criarResp.EnsureSuccessStatusCode();
            var criada = await criarResp.Content.ReadFromJsonAsync<TarefaDto>();

            // Obter
            var getResp = await _client.GetAsync($"/tarefa/{criada!.Id}");
            getResp.EnsureSuccessStatusCode();
            var obtida = await getResp.Content.ReadFromJsonAsync<TarefaDto>();
            obtida!.Titulo.Should().Be("TarefaX");

            // Atualizar
            var atualizarDto = new TarefaAtualizarDto { Titulo = "Updated", Descricao = "Updated Desc", Categoria = "Geral" };
            var putResp = await _client.PutAsJsonAsync($"/tarefa/{criada.Id}", atualizarDto);
            putResp.EnsureSuccessStatusCode();
            var atualizada = await putResp.Content.ReadFromJsonAsync<TarefaDto>();
            atualizada!.Titulo.Should().Be("Updated");

            // Deletar
            var delResp = await _client.DeleteAsync($"/tarefa/{criada.Id}");
            delResp.EnsureSuccessStatusCode();

            // Confirmar remoção
            var listarResp = await _client.GetAsync("/tarefa");
            var lista = await listarResp.Content.ReadFromJsonAsync<List<TarefaDto>>();
            lista.Should().BeEmpty();
        }

        [Fact]
        public async Task BuscarTarefas_ComFiltrosEPaginacao_DeveFuncionar()
        {
            await LimparBaseAsync();

            // Criar várias tarefas
            var tarefas = new[]
            {
                new TarefaCriarDto { Titulo = "T1", Descricao = "D1", Categoria = "C1", Prioridade = 1 },
                new TarefaCriarDto { Titulo = "T2", Descricao = "D2", Categoria = "C2", Prioridade = 2 },
                new TarefaCriarDto { Titulo = "T3", Descricao = "D3", Categoria = "C1", Prioridade = 3 },
                new TarefaCriarDto { Titulo = "T4", Descricao = "D4", Categoria = "C2", Prioridade = 1 }
            };

            foreach (var dto in tarefas)
                await _client.PostAsJsonAsync("/tarefa", dto);

            // Filtrar por categoria
            var respCat = await _client.GetAsync("/tarefa/filtro?categoria=C1");
            respCat.EnsureSuccessStatusCode();
            var resCat = await respCat.Content.ReadFromJsonAsync<List<TarefaDto>>();
            resCat.Should().HaveCount(2);
            resCat.Select(t => t.Titulo).Should().Contain(new[] { "T1", "T3" });

            // Filtrar por prioridade
            var respPri = await _client.GetAsync("/tarefa/filtro?prioridade=1");
            respPri.EnsureSuccessStatusCode();
            var resPri = await respPri.Content.ReadFromJsonAsync<List<TarefaDto>>();
            resPri.Should().HaveCount(2);
            resPri.Select(t => t.Titulo).Should().Contain(new[] { "T1", "T4" });

            // Filtrar categoria + prioridade
            var respAmbos = await _client.GetAsync("/tarefa/filtro?categoria=C2&prioridade=1");
            respAmbos.EnsureSuccessStatusCode();
            var resAmbos = await respAmbos.Content.ReadFromJsonAsync<List<TarefaDto>>();
            resAmbos.Should().HaveCount(1);
            resAmbos.First().Titulo.Should().Be("T4");

            // Paginação (2 itens por página, página 2)
            var respPag = await _client.GetAsync("/tarefa/filtro?tamanhoPagina=2&pagina=2");
            respPag.EnsureSuccessStatusCode();
            var resPag = await respPag.Content.ReadFromJsonAsync<List<TarefaDto>>();
            resPag.Should().HaveCount(2); // segunda página
        }
    }
}
