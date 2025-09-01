using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using Microsoft.EntityFrameworkCore;


namespace GerenciadorTarefas.API.Repositorios
{
    public class TarefaRepositorioPostgres : ITarefaRepositorio
    {
        private readonly TarefaDbContext _contexto;

        public TarefaRepositorioPostgres(TarefaDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Tarefa>> ListarTodasTarefas() => await _contexto.Tarefas.ToListAsync();
        public async Task<Tarefa> ObterTarefaPorId(Guid id) => await _contexto.Tarefas.FindAsync(id);
        public async Task AdicionarTarefa(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            tarefa.DataCriacao = DateTime.UtcNow;
            await _contexto.Tarefas.AddAsync(tarefa);
            await _contexto.SaveChangesAsync();
        }
        public async Task AtualizarTarefa(Tarefa tarefa)
        {
            _contexto.Tarefas.Update(tarefa);
            await _contexto.SaveChangesAsync();
        }
        public async Task RemoverTarefa(Guid id)
        {
            var tarefa = await _contexto.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                _contexto.Tarefas.Remove(tarefa);
                await _contexto.SaveChangesAsync();
            }
        }
    }
}
