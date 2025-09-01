using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Repositorios;

namespace GerenciadorTarefas.API.Servicos
{
    public class TarefaServico
    {
        private readonly ITarefaRepositorio _repositorio;

        public TarefaServico(ITarefaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<Tarefa>> ListarTarefasAsync()
        {
            return await _repositorio.ListarTodasTarefas();
        }

        public async Task<Tarefa> ObterTarefaAsync(Guid id)
        {
            return await _repositorio.ObterTarefaPorId(id);
        }

        public async Task CriarTarefaAsync(Tarefa tarefa)
        {
            await _repositorio.AdicionarTarefa(tarefa);
        }

        public async Task AtualizarTarefaAsync(Tarefa tarefa)
        {
            await _repositorio.AtualizarTarefa(tarefa);
        }

        public async Task RemoverTarefaAsync(Guid id)
        {
            await _repositorio.RemoverTarefa(id);
        }
    }
}
