using GerenciadorTarefas.API.Modelos;

namespace GerenciadorTarefas.API.Repositorios
{
    public interface ITarefaRepositorio
    {
        Task<List<Tarefa>> ListarTodasTarefas();
        Task<Tarefa> ObterTarefaPorId(Guid id);
        Task AdicionarTarefa(Tarefa tarefa);
        Task AtualizarTarefa(Tarefa tarefa);
        Task RemoverTarefa(Guid id);
    }
}
