using GerenciadorTarefas.API.Modelos.Dados;

namespace GerenciadorTarefas.API.Repositorios
{
    public class TarefaRepositorioMemoria : ITarefaRepositorio
    {
        public TarefaRepositorioMemoria()
        {
            Console.WriteLine(" Usando TarefaRepositorioMemoria");
        }

        private readonly List<Tarefa> _tarefas = new();

        // Eventos que podem ser assinados por qualquer consumidor
        public event Action<Tarefa>? TarefaCriada;
        public event Action<Tarefa>? TarefaAtualizada;
        public event Action<Guid>? TarefaRemovida;

        public Task<List<Tarefa>> ListarTodasTarefas() => Task.FromResult(_tarefas);

        public Task<Tarefa?> ObterTarefaPorId(Guid id)
        {
            var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(tarefa);
        }

        public Task<List<Tarefa>> BuscarTarefas(string? categoria = null, int? prioridade = null, int pagina = 1, int tamanhoPagina = 20)
        {
            IEnumerable<Tarefa> query = _tarefas;

            if (!string.IsNullOrEmpty(categoria))
                query = query.Where(t => t.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));

            if (prioridade.HasValue)
                query = query.Where(t => t.Prioridade == prioridade.Value);

            query = query.OrderByDescending(t => t.Prioridade)
                         .ThenBy(t => t.DataCriacao)
                         .Skip((pagina - 1) * tamanhoPagina)
                         .Take(tamanhoPagina);

            return Task.FromResult(query.ToList());
        }


        public Task AdicionarTarefa(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            tarefa.DataCriacao = DateTime.UtcNow;
            _tarefas.Add(tarefa);

            TarefaCriada?.Invoke(tarefa); // dispara evento
            return Task.CompletedTask;
        }

        public Task AtualizarTarefa(Tarefa tarefa)
        {
            var index = _tarefas.FindIndex(t => t.Id == tarefa.Id);
            if (index != -1)
            {
                _tarefas[index] = tarefa;
                TarefaAtualizada?.Invoke(tarefa); // dispara evento
            }
            return Task.CompletedTask;
        }

        public Task RemoverTarefa(Guid id)
        {
            var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
            if (tarefa != null)
            {
                _tarefas.Remove(tarefa);
                TarefaRemovida?.Invoke(id); // dispara evento
            }
            return Task.CompletedTask;
        }
    }
}
