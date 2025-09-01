using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaServico _service;

        public TarefaController(TarefaServico service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TarefaDto>>> Listar()
        {
            var tarefas = await _service.ListarAsync();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaDto>> Obter(Guid id)
        {
            var tarefa = await _service.ObterPorIdAsync(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<TarefaDto>> Criar(TarefaCriarDto dto)
        {
            var novaTarefa = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(Obter), new { id = novaTarefa.Id }, novaTarefa);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TarefaDto>> Atualizar(Guid id, TarefaAtualizarDto dto)
        {
            var tarefaAtualizada = await _service.AtualizarAsync(id, dto);
            if (tarefaAtualizada == null) return NotFound();
            return Ok(tarefaAtualizada);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(Guid id)
        {
            var deletado = await _service.DeletarAsync(id);
            if (!deletado) return NotFound();
            return NoContent();
        }
    }
}
