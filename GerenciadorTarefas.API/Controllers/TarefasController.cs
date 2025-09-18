using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaServico _service;
        private readonly LogService _logService;

        public TarefaController(TarefaServico service, LogService logService)
        {
            _service = service;
            _logService = logService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<TarefaDto>>> Listar()
        {
            var tarefas = await _service.ListarAsync();

            await _logService.RegistrarAsync(new LogEvento
            {
                Acao = "Listar Tarefas",
                Usuario = User?.Identity?.Name ?? "Anon",
                Detalhes = $"Total: {tarefas.Count}"
            });

            return Ok(tarefas);
        }

        [AllowAnonymous]
        [HttpGet("filtro")]
        public async Task<ActionResult<List<TarefaDto>>> Buscar([FromQuery] string? categoria, [FromQuery] int? prioridade,
                                                         [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20)
        {
            var tarefas = await _service.BuscarPorFiltrosAsync(categoria, prioridade, pagina, tamanhoPagina);

            await _logService.RegistrarAsync(new LogEvento
            {
                Acao = "Buscar Tarefas",
                Usuario = User?.Identity?.Name ?? "Anon",
                Detalhes = $"Filtro - Categoria: {categoria}, Prioridade: {prioridade}, Resultados: {tarefas.Count}"
            });

            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaDto>> Obter(Guid id)
        {
            var tarefa = await _service.ObterPorIdAsync(id);
            if (tarefa == null)
            {
                await _logService.RegistrarAsync(new LogEvento
                {
                    Acao = "Obter Tarefa",
                    Usuario = User?.Identity?.Name ?? "Anon",
                    Detalhes = $"Tarefa {id} não encontrada"
                });
                return NotFound();
            }

            await _logService.RegistrarAsync(new LogEvento
            {
                Acao = "Obter Tarefa",
                Usuario = User?.Identity?.Name ?? "Anon",
                Detalhes = $"Tarefa {tarefa.Titulo} encontrada"
            });

            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<TarefaDto>> Criar(TarefaCriarDto dto)
        {
            var novaTarefa = await _service.CriarAsync(dto);

            await _logService.RegistrarAsync(new LogEvento
            {
                Acao = "Criar Tarefa",
                Usuario = User?.Identity?.Name ?? "Anon",
                Detalhes = $"Tarefa {novaTarefa.Titulo} criada"
            });

            return CreatedAtAction(nameof(Obter), new { id = novaTarefa.Id }, novaTarefa);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TarefaDto>> Atualizar(Guid id, TarefaAtualizarDto dto)
        {
            var tarefaAtualizada = await _service.AtualizarAsync(id, dto);
            if (tarefaAtualizada == null)
            {
                await _logService.RegistrarAsync(new LogEvento
                {
                    Acao = "Atualizar Tarefa",
                    Usuario = User?.Identity?.Name ?? "Anon",
                    Detalhes = $"Tarefa {id} não encontrada"
                });
                return NotFound();
            }

            await _logService.RegistrarAsync(new LogEvento
            {
                Acao = "Atualizar Tarefa",
                Usuario = User?.Identity?.Name ?? "Anon",
                Detalhes = $"Tarefa {tarefaAtualizada.Titulo} atualizada"
            });

            return Ok(tarefaAtualizada);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(Guid id)
        {
            var deletado = await _service.DeletarAsync(id);
            if (!deletado)
            {
                await _logService.RegistrarAsync(new LogEvento
                {
                    Acao = "Deletar Tarefa",
                    Usuario = User?.Identity?.Name ?? "Anon",
                    Detalhes = $"Tarefa {id} não encontrada"
                });
                return NotFound();
            }

            await _logService.RegistrarAsync(new LogEvento
            {
                Acao = "Deletar Tarefa",
                Usuario = User?.Identity?.Name ?? "Anon",
                Detalhes = $"Tarefa {id} excluída"
            });

            return NoContent();
        }
    }
}
