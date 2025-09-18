using GerenciadorTarefas.API.Modelos;
using GerenciadorTarefas.API.Modelos.Dados;
using GerenciadorTarefas.API.Repositorios;

namespace GerenciadorTarefas.API.Servicos
{
    public class TarefaServico
    {
        private readonly ITarefaRepositorio _repositorio;
        private readonly IRabbitMQPublisher _rabbitMQ;

        public TarefaServico(ITarefaRepositorio repositorio, IRabbitMQPublisher rabbitMQ)
        {
            _repositorio = repositorio;
            _rabbitMQ = rabbitMQ;
        }

        public async Task<List<TarefaDto>> ListarAsync()
        {
            var tarefas = await _repositorio.ListarTodasTarefas();
            return tarefas.Select(t => new TarefaDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Categoria = t.Categoria,
                Prioridade = t.Prioridade,
                DataCriacao = t.DataCriacao
            }).ToList();
        }

        public async Task<TarefaDto?> ObterPorIdAsync(Guid id)
        {
            var t = await _repositorio.ObterTarefaPorId(id);
            if (t == null) return null;

            return new TarefaDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Categoria = t.Categoria,
                Prioridade = t.Prioridade,
                DataCriacao = t.DataCriacao
            };
        }

        public async Task<TarefaDto> CriarAsync(TarefaCriarDto dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria,
                Prioridade = dto.Prioridade
            };

            await _repositorio.AdicionarTarefa(tarefa);

            var evento = new
            {
                Tipo = "TarefaCriada",
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Categoria = tarefa.Categoria,
                Prioridade = tarefa.Prioridade,
                DataCriacao = tarefa.DataCriacao,
                Timestamp = DateTime.UtcNow
            };
            _rabbitMQ.Publish(System.Text.Json.JsonSerializer.Serialize(evento));

            return new TarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Categoria = tarefa.Categoria,
                Prioridade = tarefa.Prioridade,
                DataCriacao = tarefa.DataCriacao
            };
        }

        public async Task<TarefaDto?> AtualizarAsync(Guid id, TarefaAtualizarDto dto)
        {
            var tarefa = await _repositorio.ObterTarefaPorId(id);
            if (tarefa == null) return null;

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.Categoria = dto.Categoria;
            tarefa.Prioridade = dto.Prioridade;

            await _repositorio.AtualizarTarefa(tarefa);

            var evento = new
            {
                Tipo = "TarefaAtualizada",
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Categoria = tarefa.Categoria,
                Prioridade = tarefa.Prioridade,
                DataCriacao = tarefa.DataCriacao,
                Timestamp = DateTime.UtcNow
            };
            _rabbitMQ.Publish(System.Text.Json.JsonSerializer.Serialize(evento));

            return new TarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Categoria = tarefa.Categoria,
                Prioridade = tarefa.Prioridade,
                DataCriacao = tarefa.DataCriacao
            };
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var tarefa = await _repositorio.ObterTarefaPorId(id);
            if (tarefa == null) return false;

            await _repositorio.RemoverTarefa(id);

            var evento = new
            {
                Tipo = "TarefaRemovida",
                Id = id,
                Titulo = tarefa.Titulo,
                Categoria = tarefa.Categoria,
                Prioridade = tarefa.Prioridade,
                Timestamp = DateTime.UtcNow
            };
            _rabbitMQ.Publish(System.Text.Json.JsonSerializer.Serialize(evento));

            return true;
        }

        // --- BUSCA POR FILTROS ---
        public async Task<List<TarefaDto>> BuscarPorFiltrosAsync(string? categoria = null, int? prioridade = null, int pagina = 1, int tamanhoPagina = 20)
        {
            var tarefas = await _repositorio.BuscarTarefas(categoria, prioridade, pagina, tamanhoPagina);

            return tarefas.Select(t => new TarefaDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Descricao = t.Descricao,
                Categoria = t.Categoria,
                Prioridade = t.Prioridade,
                DataCriacao = t.DataCriacao
            }).ToList();
        }
    }
}
