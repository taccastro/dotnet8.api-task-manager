using GerenciadorTarefas.API.Modelos.Dados;

namespace GerenciadorTarefas.API.Repositorios
{
    public interface ITarefaRepositorio
    {
        Task<List<Tarefa>> ListarTodasTarefas();
        Task<Tarefa?> ObterTarefaPorId(Guid id);
        Task AdicionarTarefa(Tarefa tarefa);
        Task AtualizarTarefa(Tarefa tarefa);
        Task RemoverTarefa(Guid id);
        Task<List<Tarefa>> BuscarTarefas(string? categoria = null, int? prioridade = null, int pagina = 1, int tamanhoPagina = 20);

    }
}
