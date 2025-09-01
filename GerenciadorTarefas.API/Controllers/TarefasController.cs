using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly TarefaServico _servico;

        public TarefasController(TarefaServico servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var tarefas = await _servico.ListarTarefasAsync();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obter(Guid id)
        {
            var tarefa = await _servico.ObterTarefaAsync(id);
            return tarefa != null ? Ok(tarefa) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Tarefa tarefa)
        {
            await _servico.CriarTarefaAsync(tarefa);
            return CreatedAtAction(nameof(Obter), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] Tarefa tarefa)
        {
            if (id != tarefa.Id) return BadRequest();
            await _servico.AtualizarTarefaAsync(tarefa);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            await _servico.RemoverTarefaAsync(id);
            return NoContent();
        }
    }
}
