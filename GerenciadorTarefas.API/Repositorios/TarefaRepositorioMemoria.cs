using GerenciadorTarefas.API.Modelos;

namespace GerenciadorTarefas.API.Repositorios
{
    public class TarefaRepositorioMemoria : ITarefaRepositorio
    {
        public TarefaRepositorioMemoria()
        {
            Console.WriteLine("🚀 Usando TarefaRepositorioMemoria");
        }

        private readonly List<Tarefa> _tarefas = new();

        public Task<List<Tarefa>> ListarTodasTarefas()
        {
            return Task.FromResult(_tarefas);
        }

        public Task<Tarefa> ObterTarefaPorId(Guid id)
        {
            var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(tarefa);
        }

        public Task AdicionarTarefa(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            tarefa.DataCriacao = DateTime.UtcNow;
            _tarefas.Add(tarefa);
            return Task.CompletedTask;
        }

        public Task AtualizarTarefa(Tarefa tarefa)
        {
            var index = _tarefas.FindIndex(t => t.Id == tarefa.Id);
            if (index != -1)
            {
                _tarefas[index] = tarefa;
            }
            return Task.CompletedTask;
        }

        public Task RemoverTarefa(Guid id)
        {
            var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
            if (tarefa != null)
            {
                _tarefas.Remove(tarefa);
            }
            return Task.CompletedTask;
        }
    }
}
